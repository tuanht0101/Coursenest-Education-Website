using AutoMapper;
using AutoMapper.QueryableExtensions;
using CommonLibrary.API.Models;
using CommonLibrary.API.Utilities.APIs;
using Humanizer;
using Library.API.DTOs;
using Library.API.DTOs.Lessons;
using Library.API.Infrastructure.Contexts;
using Library.API.Infrastructure.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace Library.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LessonsController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly DataContext _context;

		public LessonsController(IMapper mapper, DataContext context)
		{
			_mapper = mapper;
			_context = context;
		}


		// GET: /lessons
		[HttpGet]
		public async Task<ActionResult<LessonResult[]>> GetAll(
			[BindRequired][FromQuery] int courseId)
		{
			var userId = GetUserIdOrDefault();
			var isAdmin = IsAdmin();

			var course = await _context.Courses
				.Include(x => x.Lessons)
				.FirstOrDefaultAsync(x =>
					x.CourseId == courseId &&
					(isAdmin || x.IsApproved || x.PublisherUserId == userId));
			if (course == null)
				return NotFound("CourseId does not exist or you're not authorized.");

			var results = course.Lessons
				.Select(x => _mapper.Map<LessonResult>(x))
				.ToArray();

			return results;
		}

		// GET: /lessons/5
		[HttpGet("{lessonId}")]
		public async Task<ActionResult<LessonResult>> Get(
			int lessonId)
		{
			var userId = GetUserIdOrDefault();
			var isAdmin = IsAdmin();

			var result = await _context.Lessons
				.Where(x =>
					x.LessonId == lessonId &&
					(isAdmin || x.Course.IsApproved || x.Course.PublisherUserId == userId))
				.ProjectTo<LessonResult>(_mapper.ConfigurationProvider)
				.FirstOrDefaultAsync();
			if (result == null)
				return NotFound("LessonId does not exist or you're not authorized.");

			return result;
		}


		// POST: /lessons
		[HttpPost]
		[Authorize(Roles = nameof(RoleType.Publisher))]
		public async Task<ActionResult> Create(
			[FromBody] CreateLesson body)
		{
			var userId = GetUserId();

			var course = await _context.Courses
				.Include(x => x.Lessons)
				.FirstOrDefaultAsync(x =>
					x.CourseId == body.CourseId &&
					x.PublisherUserId == userId);
			if (course == null)
				return NotFound("CourseId does not exist or you're not authorized.");

			var lesson = _mapper.Map<Lesson>(body);

			var max = course.Lessons
				.Select(x => x.Order)
				.OrderByDescending(x => x)
				.FirstOrDefault();
			lesson.OrderNumerator = max == default ? 1 : ((int)max + 1);
			lesson.OrderDenominator = 1;

			course.Lessons.Add(lesson);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetAll), new { body.CourseId }, null);
		}


		// PUT: /lessons/5
		[HttpPut("{lessonId}")]
		[Authorize(Roles = nameof(RoleType.Publisher))]
		public async Task<ActionResult> Update(
			int lessonId,
			[FromBody] UpdateLesson body)
		{
			var userId = GetUserId();

			var lesson = await _context.Lessons
				.FirstOrDefaultAsync(x =>
					x.LessonId == lessonId &&
					x.Course.PublisherUserId == userId);
			if (lesson == null)
				return NotFound("LessonId does not exist or you're not authorized.");

			_mapper.Map(body, lesson);
			await _context.SaveChangesAsync();

			return NoContent();
		}


		// DELETE: /lessons/5
		[HttpDelete("{lessonId}")]
		[Authorize(Roles = nameof(RoleType.Publisher))]
		public async Task<ActionResult> Delete(
			int lessonId)
		{
			var userId = GetUserId();

			var result = await _context.Lessons
				.Where(x => x.LessonId == lessonId && x.Course.PublisherUserId == userId)
				.ExecuteDeleteAsync();
			if (result == 0)
				return NotFound("LessonId does not exist or you're not authorized.");

			return NoContent();
		}


		// PUT: /lessons/5/order
		[HttpPut("{lessonId}/order")]
		[Authorize(Roles = nameof(RoleType.Publisher))]
		public async Task<ActionResult> ChangeOrder(
			int lessonId,
			[FromBody] ChangeOrder body)
		{
			var userId = GetUserId();

			if (lessonId == body.ToId)
				return BadRequest("Both LessonId is the same.");

			var course = await _context.Courses
				.Include(x => x.Lessons)
				.FirstOrDefaultAsync(x =>
					x.PublisherUserId == userId &&
					x.Lessons.Any(x => x.LessonId == lessonId) &&
					x.Lessons.Any(x => x.LessonId == body.ToId));
			if (course == null)
				return NotFound("Course contains both LessonIds does not exist or you're not authorized.");

			var from = course.Lessons.First(x => x.LessonId == lessonId);
			var to = course.Lessons.First(x => x.LessonId == body.ToId);

			if (body.IsBefore)
			{
				var before = course.Lessons
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
				var after = course.Lessons
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
