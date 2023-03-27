using AutoMapper;
using AutoMapper.QueryableExtensions;
using CommonLibrary.API.MessageBus.Commands;
using CommonLibrary.API.MessageBus.Responses;
using CommonLibrary.API.Models;
using CommonLibrary.API.Utilities.APIs;
using Library.API.DTOs;
using Library.API.DTOs.Units;
using Library.API.Infrastructure.Contexts;
using Library.API.Infrastructure.Entities;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using ExamResult = Library.API.DTOs.Units.ExamResult;

namespace Library.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UnitsController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly DataContext _context;
		private readonly IRequestClient<CheckEnrollment> _checkEnrollmentClient;

		public UnitsController(
			IMapper mapper,
			DataContext context,
			IRequestClient<CheckEnrollment> checkEnrollmentClient)
		{
			_mapper = mapper;
			_context = context;
			_checkEnrollmentClient = checkEnrollmentClient;
		}


		// GET: /units
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UnitResult>))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetAll(
			[BindRequired][FromQuery] int lessonId)
		{
			var userId = GetUserIdOrDefault();
			var isAdmin = IsAdmin();

			var lesson = await _context.Lessons
				.Include(x => x.Units)
				.FirstOrDefaultAsync(x =>
					x.LessonId == lessonId &&
					(isAdmin || x.Course.IsApproved || x.Course.PublisherUserId == userId));
			if (lesson == null)
				return NotFound("LessonId does not exist or you're not authorized.");

			var results = lesson.Units
				.Select(x => _mapper.Map<UnitResult>(x))
				.ToArray();

			return Ok(results);
		}

		// GET: /units/count
		[HttpGet("count")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetCountByCourseId(
			[BindRequired][FromQuery] int courseId)
		{
			var userId = GetUserIdOrDefault();
			var isAdmin = IsAdmin();

			var exists = await _context.Courses
				.AnyAsync(x =>
					x.CourseId == courseId &&
					(isAdmin || x.IsApproved || x.PublisherUserId == userId));
			if (!exists)
				return NotFound("CourseId does not exist or you're not authorized.");

			var count = await _context.Units
				.CountAsync(x => x.Lesson.CourseId == courseId);

			return Ok(count);
		}

		// GET: /units/5
		[HttpGet("{unitId}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UnitResult))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Get(
			int unitId)
		{
			var userId = GetUserIdOrDefault();
			var isAdmin = IsAdmin();

			var result = await _context.Units
				.Where(x =>
					x.UnitId == unitId &&
					(isAdmin || x.Lesson.Course.IsApproved || x.Lesson.Course.PublisherUserId == userId))
				.ProjectTo<UnitResult>(_mapper.ConfigurationProvider)
				.FirstOrDefaultAsync();
			if (result == null)
				return NotFound("UnitId does not exist or you're not authorized.");

			return Ok(result);
		}

		// GET: /units/5/material
		[HttpGet("{unitId}/material")]
		[Authorize]
		public async Task<ActionResult<MaterialResult>> GetMaterial(
			int unitId)
		{
			var userId = GetUserId();
			var isAdmin = IsAdmin();

			var result = await _context.Materials
				.Where(x => x.UnitId == unitId)
				.ProjectTo<MaterialResult>(_mapper.ConfigurationProvider)
				.FirstOrDefaultAsync();
			if (result == null)
				return NotFound("Material with UnitId does not exist or you're not authorized.");

			var isPublisher = result.PublisherUserId == userId;

			if (!isPublisher && !isAdmin)
			{
				var request = new CheckEnrollment()
				{
					CourseId = result.CourseId,
					StudentUserId = userId
				};
				var response = await _checkEnrollmentClient
					.GetResponse<Existed, NotFound>(request);

				if (response.Is(out Response<NotFound>? notFoundResponse))
				{
					return NotFound(notFoundResponse!.Message.Message);
				}
			}
			
			return result;
		}

		// GET: /units/5/exam
		[HttpGet("{unitId}/exam")]
		[Authorize]
		public async Task<ActionResult<ExamResult>> GetExam(
			int unitId)
		{
			var userId = GetUserId();
			var isAdmin = IsAdmin();

			var result = await _context.Exams
				.Where(x =>
					x.UnitId == unitId)
				.ProjectTo<ExamResult>(_mapper.ConfigurationProvider)
				.FirstOrDefaultAsync();
			if (result == null)
				return NotFound("Exam with UnitId does not exist or you're not authorized.");

			return result;
		}


		// POST: /units/material
		[HttpPost("material")]
		[Authorize(Roles = nameof(RoleType.Publisher))]
		public async Task<ActionResult> CreateMaterial(
			[FromBody] CreateMaterial body)
		{
			var userId = GetUserId();

			var lesson = await _context.Lessons
				.Include(x => x.Units)
				.FirstOrDefaultAsync(x =>
					x.LessonId == body.LessonId &&
					x.Course.PublisherUserId == userId);
			if (lesson == null)
				return NotFound("LessonId does not exist or you're not authorized.");

			var material = _mapper.Map<Material>(body);

			var max = lesson.Units
				.Select(x => x.Order)
				.OrderByDescending(x => x)
				.FirstOrDefault();
			material.OrderNumerator = max == default ? 1 : ((int)max + 1);
			material.OrderDenominator = 1;

			lesson.Units.Add(material);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetMaterial), new { material.UnitId }, null);
		}

		// POST: /units/exam
		[HttpPost("exam")]
		[Authorize(Roles = nameof(RoleType.Publisher))]
		public async Task<ActionResult> CreateExam(
			[FromBody] CreateExam body)
		{
			var userId = GetUserId();

			var lesson = await _context.Lessons
				.Include(x => x.Units)
				.FirstOrDefaultAsync(x =>
					x.LessonId == body.LessonId &&
					x.Course.PublisherUserId == userId);
			if (lesson == null)
				return NotFound("LessonId does not exist or you're not authorized.");

			var exam = _mapper.Map<Exam>(body);

			var max = lesson.Units
				.Select(x => x.Order)
				.OrderByDescending(x => x)
				.FirstOrDefault();
			exam.OrderNumerator = max == default ? 1 : ((int)max + 1);
			exam.OrderDenominator = 1;

			lesson.Units.Add(exam);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetExam), new { exam.UnitId }, null);
		}


		// PUT: /units/5/material
		[HttpPut("{unitId}/material")]
		[Authorize(Roles = nameof(RoleType.Publisher))]
		public async Task<ActionResult> UpdateMaterial(
			int unitId,
			[FromBody] UpdateMaterial body)
		{
			var userId = GetUserId();

			var material = await _context.Materials
				.FirstOrDefaultAsync(x =>
					x.UnitId == unitId &&
					x.Lesson.Course.PublisherUserId == userId);
			if (material == null)
				return NotFound("Material with UnitId does not exist or you're not authorized.");

			_mapper.Map(body, material);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		// PUT: /units/5/exam
		[HttpPut("{unitId}/exam")]
		[Authorize(Roles = nameof(RoleType.Publisher))]
		public async Task<ActionResult> UpdateExam(
			int unitId,
			[FromBody] UpdateUnit body)
		{
			var userId = GetUserId();

			var exam = await _context.Exams
				.FirstOrDefaultAsync(x =>
					x.UnitId == unitId &&
					x.Lesson.Course.PublisherUserId == userId);
			if (exam == null)
				return NotFound("Exam with UnitId does not exist or you're not authorized.");

			_mapper.Map(body, exam);
			await _context.SaveChangesAsync();

			return NoContent();
		}


		// DELETE: /units/5
		[HttpDelete("{unitId}")]
		[Authorize(Roles = nameof(RoleType.Publisher))]
		public async Task<ActionResult> Delete(
			int unitId)
		{
			var userId = GetUserId();

			var result = await _context.Units
				.Where(x => x.UnitId == unitId && x.Lesson.Course.PublisherUserId == userId)
				.ExecuteDeleteAsync();
			if (result == 0)
				return NotFound("UnitId does not exist or you're not authorized.");

			return NoContent();
		}


		// PUT: /units/5/order
		[HttpPut("{unitId}/order")]
		public async Task<ActionResult> ChangeOrder(
			int unitId,
			[FromBody] ChangeOrder body)
		{
			var userId = GetUserId();

			if (unitId == body.ToId)
				return BadRequest("Both UnitId is the same.");

			var lesson = await _context.Lessons
				.Include(x => x.Units)
				.FirstOrDefaultAsync(x =>
					x.Course.PublisherUserId == userId &&
					x.Units.Any(x => x.UnitId == unitId) &&
					x.Units.Any(x => x.UnitId == body.ToId));
			if (lesson == null)
				return NotFound("Lesson contains both LessonIds does not exist or you're not authorized.");

			var from = lesson.Units.First(x => x.UnitId == unitId);
			var to = lesson.Units.First(x => x.UnitId == body.ToId);

			if (body.IsBefore)
			{
				var before = lesson.Units
					.Where(x => x.Order < to.Order)
					.OrderByDescending(x => x.Order)
					.FirstOrDefault();
				if (before == from)
					return BadRequest("Nothing has changed.");

				from.OrderNumerator = (before == null ? 0 : before.OrderNumerator) + to.OrderNumerator;
				from.OrderDenominator = (before == null ? 1 : before.OrderDenominator) + to.OrderDenominator;
			}
			else
			{
				var after = lesson.Units
					.Where(x => x.Order > to.Order)
					.OrderBy(x => x.Order)
					.FirstOrDefault();
				if (after == from)
					return BadRequest("Nothing has changed.");

				from.OrderNumerator = after == null ? ((int)to.Order + 1) : after.OrderNumerator + to.OrderNumerator;
				from.OrderDenominator = after == null ? 1 : after.OrderDenominator + to.OrderDenominator;
			}

			await _context.SaveChangesAsync();

			return NoContent();
		}


		// POST: /units/questions
		[HttpPost("questions")]
		[Authorize(Roles = nameof(RoleType.Publisher))]
		public async Task<ActionResult> CreateQuestion(
			[FromBody] CreateQuestion body)
		{
			var userId = GetUserId();

			var exam = await _context.Exams
				.Include(x => x.Questions)
				.FirstOrDefaultAsync(x =>
					x.UnitId == body.ExamUnitId &&
					x.Lesson.Course.PublisherUserId == userId);
			if (exam == null)
				return NotFound("Exam with UnitId does not exist or you're not authorized.");

			var question = _mapper.Map<Question>(body);

			exam.Questions.Add(question);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetExam), new { exam.UnitId }, null);
		}


		// PUT: /units/questions/5
		[HttpPut("questions/{questionId}")]
		[Authorize(Roles = nameof(RoleType.Publisher))]
		public async Task<ActionResult<QuestionResult>> UpdateQuestion(
			int questionId,
			[FromBody] UpdateQuestion body)
		{
			var userId = GetUserId();

			var question = await _context.Questions
				.Include(x => x.Choices)
				.FirstOrDefaultAsync(x =>
					x.QuestionId == questionId &&
					x.ExamUnitId == body.ExamUnitId &&
					x.Exam.Lesson.Course.PublisherUserId == userId);
			if (question == null)
				return NotFound("Exam with UnitId or QuestionId does not exist or you're not authorized.");

			_mapper.Map(body, question);

			await _context.SaveChangesAsync();

			return NoContent();
		}


		// DELETE: /units/questions/5
		[HttpDelete("questions/{questionId}")]
		public async Task<ActionResult> DeleteQuestion(
			int questionId)
		{
			var userId = GetUserId();

			var result = await _context.Questions
				.Where(x =>
					x.QuestionId == questionId &&
					x.Exam.Lesson.Course.PublisherUserId == userId)
				.ExecuteDeleteAsync();
			if (result == 0)
				return NotFound("QuestionId does not exist or you're not authorized.");

			return NoContent();
		}


		private int GetUserId()
		{
			return ClaimUtilities.GetUserId(User.Claims);
		}

		private int GetUserIdOrDefault()
		{
			return User.Claims.Any() ? ClaimUtilities.GetUserId(User.Claims) : 0;
		}

		private bool IsAdmin()
		{
			return ClaimUtilities.GetRoles(User.Claims).Any(x => x == RoleType.Admin);
		}
	}
}
