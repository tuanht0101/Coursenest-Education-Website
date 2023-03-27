using AutoMapper;
using AutoMapper.QueryableExtensions;
using CommonLibrary.API.MessageBus.Commands;
using CommonLibrary.API.MessageBus.Responses;
using CommonLibrary.API.Models;
using CommonLibrary.API.Utilities.APIs;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserData.API.DTOs;
using UserData.API.Infrastructure.Contexts;
using UserData.API.Infrastructure.Entities;

namespace UserData.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EnrollmentsController : ControllerBase
{
	private readonly IMapper _mapper;
	private readonly DataContext _context;
	private readonly IRequestClient<GetCourseTier> _getCourseTierClient;
	private readonly IRequestClient<CheckUnits> _checkUnitsClient;

	public EnrollmentsController(
		IMapper mapper,
		DataContext context,
		IRequestClient<GetCourseTier> getCourseTierClient,
		IRequestClient<CheckUnits> checkUnitsClient)
	{
		_mapper = mapper;
		_context = context;
		_getCourseTierClient = getCourseTierClient;
		_checkUnitsClient = checkUnitsClient;
	}


	// GET: /enrollments
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<EnrollmentResult>))]
	public async Task<IActionResult> GetAll()
	{
		var userId = GetUserId();

		var results = await _context.Enrollments
			.Where(x => x.StudentUserId == userId)
			.ProjectTo<EnrollmentResult>(_mapper.ConfigurationProvider)
			.ToArrayAsync();

		return Ok(results);
	}

	// GET: /enrollments/5
	[HttpGet("{enrollmentId}")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EnrollmentDetailResult))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Get(
		int enrollmentId)
	{
		var userId = GetUserId();

		var result = await _context.Enrollments
			.ProjectTo<EnrollmentDetailResult>(_mapper.ConfigurationProvider)
			.FirstOrDefaultAsync(x =>
				x.EnrollmentId == enrollmentId &&
				x.StudentUserId == userId);
		if (result == null)
			return NotFound($"EnrollmentId does not exist.");

		return Ok(result);
	}


	// POST: /enrollments
	[HttpPost]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status409Conflict)]
	public async Task<IActionResult> Create(
		[FromBody] int courseId)
	{
		var userId = GetUserId();

		var enrollments = await _context.Enrollments
			.AsNoTracking()
			.Where(x => x.StudentUserId == userId)
			.Select(x => new { x.CourseId, x.Completed })
			.ToListAsync();
		if (enrollments.Any(x => x.CourseId == courseId))
			return Conflict($"Already enrolled.");
		if (enrollments.Count(x => x.Completed != null) >= 3)
			return Conflict("Cannot enroll more than 3 courses.");

		var request = new GetCourseTier()
		{
			Id = courseId,
			IsApproved = true
		};
		var response = await _getCourseTierClient
			.GetResponse<CourseTierResult, NotFound>(request);
		if (response.Is(out Response<NotFound>? notFoundResponse))
		{
			return NotFound(notFoundResponse!.Message.Message);
		}
		if (!response.Is(out Response<CourseTierResult>? courseTierResult) ||
			courseTierResult == null)
		{
			return StatusCode(StatusCodes.Status500InternalServerError);
		}

		if (courseTierResult.Message.Tier == CourseTier.Premium &&
			!User.IsInRole(nameof(RoleType.Student)))
		{
			return Conflict("User does not have Student role.");
		}

		var enrollment = new Enrollment()
		{
			CourseId = courseId,
			StudentUserId = userId
		};
		_context.Enrollments.Add(enrollment);

		await _context.SaveChangesAsync();

		return CreatedAtAction(nameof(Get), new { enrollment.EnrollmentId }, null);
	}


	// DELETE: /enrollments/5
	[HttpDelete("{enrollmentId}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Delete(
		int enrollmentId)
	{
		var userId = GetUserId();

		var affected = await _context.Enrollments
			.Where(x =>
				x.EnrollmentId == enrollmentId &&
				x.StudentUserId == userId)
			.ExecuteDeleteAsync();
		if (affected == 0)
			return NotFound("EnrollmentId does not exist or you're not authorized.");

		return NoContent();
	}


	// POST: /enrollments/material
	[HttpPost("material")]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status409Conflict)]
	public async Task<ActionResult> CompleteMaterial(
		[FromBody] CompleteMaterial body)
	{
		var userId = GetUserId();

		var enrollment = await _context.Enrollments
			.AsNoTracking()
			.Include(x => x.CompletedUnits)
			.FirstOrDefaultAsync(x =>
				x.StudentUserId == userId &&
				x.EnrollmentId == body.EnrollmentId &&
				x.Completed == null);
		if (enrollment == null)
			return NotFound("EnrollmentId does not exist or completed or you're not authorized.");
		if (enrollment.CompletedUnits.Any(x => x.UnitId == body.UnitId))
			return Conflict($"UnitId existed.");

		var request = new CheckUnits()
		{
			Queries = new[]
			{
				new CheckUnits.Query()
				{
					Id = body.UnitId,
					IsExam = false
				}
			}
		};
		var response = await _checkUnitsClient
			.GetResponse<Existed, NotFound>(request);
		if (response.Is(out Response<NotFound>? notFoundResponse))
		{
			return NotFound(notFoundResponse!.Message.Message);
		}

		var completedUnit = new CompletedUnit()
		{
			UnitId = body.UnitId,
			EnrollmentId = body.EnrollmentId
		};
		_context.CompletedUnits.Add(completedUnit);

		await _context.SaveChangesAsync();

		return CreatedAtAction(nameof(Get), new { body.EnrollmentId }, null);
	}


	private int GetUserId()
	{
		return ClaimUtilities.GetUserId(User.Claims);
	}
}
