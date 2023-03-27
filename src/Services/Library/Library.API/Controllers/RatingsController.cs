using AutoMapper;
using AutoMapper.QueryableExtensions;
using CommonLibrary.API.Utilities.APIs;
using Library.API.DTOs.Ratings;
using Library.API.Infrastructure.Contexts;
using Library.API.Infrastructure.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RatingsController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly DataContext _context;

		public RatingsController(IMapper mapper, DataContext context)
		{
			_mapper = mapper;
			_context = context;
		}


		// GET: /ratings
		[HttpGet]
		public async Task<ActionResult<RatingResult[]>> GetAll(
			[FromQuery] RatingQuery query)
		{
			var results = await _context.Ratings
				.Where(x =>
					(query.CourseId == null || query.CourseId == x.CourseId) &&
					(query.UserId == null || query.UserId == x.UserId))
				.OrderByDescending(x => x.Stars)
				.Skip((query.PageNumber - 1) * query.PageSize)
				.Take(query.PageSize)
				.ProjectTo<RatingResult>(_mapper.ConfigurationProvider)
				.ToArrayAsync();

			return results;
		}

		// POST: /ratings
		[HttpPost]
		[Authorize]
		public async Task<ActionResult> Create(
			[FromBody] CreateRating body)
		{
			var userId = GetUserId();

			var course = await _context.Courses.FindAsync(body.CourseId);
			if (course == null)
				return NotFound("CourseId does not exist.");

			var exists = await _context.Ratings
				.AnyAsync(x => x.UserId == userId && x.CourseId == body.CourseId);
			if (exists)
				return Conflict("Already rated this course.");

			var rating = _mapper.Map<Rating>(body);
			rating.UserId = userId;

			course.RatingAverage = (course.RatingTotal * course.RatingAverage + body.Stars) / (course.RatingTotal + 1);
			course.RatingTotal++;

			_context.Ratings.Add(rating);
			await _context.SaveChangesAsync();

			return CreatedAtAction(
				nameof(GetAll),
				new RatingQuery() { CourseId = body.CourseId, UserId = userId },
				null);
		}

		// DELETE: /ratings
		[HttpDelete]
		[Authorize]
		public async Task<ActionResult> Delete(
			int courseId)
		{
			var userId = GetUserId();

			var result = await _context.Ratings
				.Where(x => x.CourseId == courseId && x.UserId == userId)
				.ExecuteDeleteAsync();
			if (result == 0)
				return NotFound("CourseId does not exist or User hasn't rated yet.");

			return NoContent();
		}


		private int GetUserId()
		{
			return ClaimUtilities.GetUserId(User.Claims);
		}
	}
}
