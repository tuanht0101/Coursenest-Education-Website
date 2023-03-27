using AutoMapper;
using AutoMapper.QueryableExtensions;
using CommonLibrary.API.Constants;
using CommonLibrary.API.Models;
using CommonLibrary.API.Utilities.APIs;
using CommonLibrary.API.Validations;
using Library.API.DTOs.Courses;
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
	public class CoursesController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly DataContext _context;

		public CoursesController(IMapper mapper, DataContext context)
		{
			_mapper = mapper;
			_context = context;
		}


		// GET: /courses
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CourseResults))]
		public async Task<IActionResult> GetAll(
			[FromQuery] CourseQuery query)
		{
			var userId = User.Claims.Any() ? GetUserId() : 0;
			var isAdmin = GetRoles().Any(x => x == RoleType.Admin);

			var dbQuery = _context.Courses
				.Where(x =>
					(isAdmin || x.IsApproved || x.PublisherUserId == userId) &&
					(query.IsApproved == x.IsApproved) &&
					(string.IsNullOrWhiteSpace(query.Title) || x.Title.Contains(query.Title)) &&
					(query.TopicId == null || query.TopicId == x.TopicId) &&
					(query.PublisherUserId == null || query.PublisherUserId == x.PublisherUserId));

			var searchQuery = dbQuery
				.Skip((query.PageNumber - 1) * query.PageSize)
				.Take(query.PageSize);

			searchQuery = query.SortBy switch
			{
				SortBy.Created => searchQuery.OrderByDescending(x => x.Created),
				SortBy.LastModified => searchQuery.OrderByDescending(x => x.LastModified),
				SortBy.Title => searchQuery.OrderBy(x => x.Title),
				_ => searchQuery.OrderByDescending(x => x.RatingAverage),
			};

			var result = new CourseResults
			{
				Queried = await searchQuery
					.ProjectTo<CourseResult>(_mapper.ConfigurationProvider)
					.ToArrayAsync(),
				Total = await dbQuery
					.CountAsync()
			};

			return Ok(result);
		}

		// GET: /courses/5
		[HttpGet("{courseId}")]
		public async Task<ActionResult<CourseDetailedResult>> Get(
			int courseId)
		{
			var userId = User.Claims.Any() ? GetUserId() : 0;
			var isAdmin = GetRoles().Any(x => x == RoleType.Admin);

			var result = await _context.Courses
				.Where(x =>
					x.CourseId == courseId &&
					(isAdmin || x.IsApproved || x.PublisherUserId == userId))
				.ProjectTo<CourseDetailedResult>(_mapper.ConfigurationProvider)
				.FirstOrDefaultAsync();
			if (result == null)
				return NotFound("Course does not exist or you're not authorized.");

			return result;
		}


		// POST: /courses
		[HttpPost]
		[Authorize(Roles = nameof(RoleType.Publisher))]
		public async Task<ActionResult> Create(
			[FromBody] CreateCourse body)
		{
			var userId = GetUserId();

			if (body.TopicId != null)
			{
				var exists = await _context.Topics
					.AnyAsync(x => x.TopicId == body.TopicId);
				if (!exists)
					return NotFound("TopicId does not exist.");
			}

			var course = _mapper.Map<Course>(body);
			course.PublisherUserId = userId;

			_context.Courses.Add(course);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(Get), new { course.CourseId }, null);
		}


		// PUT: /courses/5
		[HttpPut("{courseId}")]
		[Authorize(Roles = nameof(RoleType.Publisher))]
		public async Task<ActionResult> Update(
			int courseId,
			[FromBody] UpdateCourse body)
		{
			var userId = GetUserId();

			var course = await _context.Courses
				.FirstOrDefaultAsync(x => x.CourseId == courseId && x.PublisherUserId == userId);
			if (course == null)
				return NotFound("Course does not exist or you're not authorized.");

			_mapper.Map(body, course);
			await _context.SaveChangesAsync();

			return NoContent();
		}


		// DELETE: /courses/5
		[HttpDelete("{courseId}")]
		[Authorize(Roles = nameof(RoleType.Publisher))]
		public async Task<ActionResult> Delete(
			int courseId)
		{
			var userId = GetUserId();

			var result = await _context.Courses
				.Where(x => x.CourseId == courseId && x.PublisherUserId == userId)
				.ExecuteDeleteAsync();
			if (result == 0)
				return NotFound("Course does not exist or you're not authorized.");

			return NoContent();
		}


		// PUT: /courses/5/cover
		[HttpPut("{courseId}/cover")]
		[Authorize(Roles = nameof(RoleType.Publisher))]
		public async Task<ActionResult> UpdateCover(
			int courseId,
			[BindRequired][MaxSize(0, 2 * 1024 * 1024)][ImageExtension] IFormFile formFile)
		{
			var userId = GetUserId();

			var course = await _context.Courses
				.Include(x => x.Cover)
				.FirstOrDefaultAsync(x => x.CourseId == courseId && x.PublisherUserId == userId);
			if (course == null)
				return NotFound("Course does not exist or you're not authorized.");

			using var memoryStream = new MemoryStream();
			await formFile.CopyToAsync(memoryStream);
			var extension = Path.GetExtension(formFile.FileName).ToLowerInvariant();

			course.Cover = new CourseCover()
			{
				MediaType = FormFileContants.ExtensionMIMEs.GetValueOrDefault(extension)!,
				Data = memoryStream.ToArray(),
				CourseId = courseId
			};
			course.LastModified = DateTime.Now;

			await _context.SaveChangesAsync();

			return NoContent();
		}


		// PUT: /courses/5/approve
		[HttpPut("{courseId}/approve")]
		[Authorize(Roles = nameof(RoleType.Admin))]
		public async Task<ActionResult> Approve(
			int courseId)
		{
			var course = await _context.Courses
				.FirstOrDefaultAsync(x => x.CourseId == courseId && !x.IsApproved);
			if (course == null)
				return NotFound("Course does not exist, or is approved or you're not authorized.");

			course.IsApproved = true;

			await _context.SaveChangesAsync();

			return NoContent();
		}


		private int GetUserId()
		{
			return ClaimUtilities.GetUserId(User.Claims);
		}

		private IEnumerable<RoleType> GetRoles()
		{
			return ClaimUtilities.GetRoles(User.Claims);
		}
	}
}
