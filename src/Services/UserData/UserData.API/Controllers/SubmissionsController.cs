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
public class SubmissionsController : ControllerBase
{
	private readonly IMapper _mapper;
	private readonly DataContext _context;
	private readonly IRequestClient<GetExam> _getExamClient;

	public SubmissionsController(
		IMapper mapper,
		DataContext context,
		IRequestClient<GetExam> getExamClient)
	{
		_mapper = mapper;
		_context = context;
		_getExamClient = getExamClient;
	}


	// GET: /submissions/ongoing
	[HttpGet("ongoing")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SubmissionOngoingResult))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetOngoing()
	{
		var userId = GetUserId();

		var result = await _context.Submissions
			.Where(x =>
				x.StudentUserId == userId &&
				DateTime.Now < x.Deadline &&
				DateTime.Now < x.Ended)
			.ProjectTo<SubmissionOngoingResult>(_mapper.ConfigurationProvider)
			.FirstOrDefaultAsync();
		if (result == null)
			return NotFound("There is no ongoing examination.");

		return Ok(result);
	}

	// POST: /submissions
	[HttpPost]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status409Conflict)]
	public async Task<IActionResult> StartExam(
		[FromBody] StartExam body)
	{
		var userId = GetUserId();

		var enrollmentExists = await _context.Enrollments
			.AnyAsync(x =>
				x.EnrollmentId == body.EnrollmentId &&
				x.Completed != null &&
				x.StudentUserId == userId);
		if (!enrollmentExists)
			return NotFound("EnrollmentId does not exist" +
				" or is completed" +
				" or you're not authorized.");

		var now = DateTime.Now;
		var ongoingExists = await _context.Submissions
			.AnyAsync(x =>
				x.StudentUserId == userId &&
				now < x.Deadline &&
				now < x.Ended);
		if (ongoingExists)
			return Conflict("There is an ongoing examination or you're not authorized.");

		var request = new GetExam()
		{
			UnitId = body.ExamUnitId
		};
		var response = await _getExamClient
			.GetResponse<ExamResult, NotFound>(request);
		if (response.Is(out Response<NotFound>? notFoundResponse))
		{
			return NotFound(notFoundResponse!.Message.Message);
		}
		if (!response.Is(out Response<ExamResult>? examResult) ||
			examResult == null)
		{
			return StatusCode(StatusCodes.Status500InternalServerError);
		}

		var submission = _mapper.Map<Submission>(examResult.Message);
		submission.StudentUserId = userId;
		submission.EnrollmentId = body.EnrollmentId;
		_context.Submissions.Add(submission);

		await _context.SaveChangesAsync();

		return CreatedAtAction(nameof(GetOngoing), null);
	}


	// GET: /submissions
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SubmissionResults))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetAll(
		[FromQuery] int enrollmentId)
	{
		var userId = GetUserId();

		var exists = await _context.Enrollments
			.AnyAsync(x =>
				x.EnrollmentId == enrollmentId &&
				x.StudentUserId == userId);
		if (!exists)
			return NotFound("EnrollmentId does not exist or you're not authorized.");

		var submissions = await _context.Submissions
			.Where(x =>
				x.EnrollmentId == enrollmentId &&
				x.StudentUserId == userId)
			.ProjectTo<SubmissionResults.Submission>(_mapper.ConfigurationProvider)
			.ToArrayAsync();

		var result = new SubmissionResults()
		{
			Queried = submissions,
			Total = submissions.Length
		};

		return Ok(result);
	}

	// GET: /submissions/instructor
	[HttpGet("instructor")]
	[Authorize(Roles = nameof(RoleType.Instructor))]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SubmissionResults))]
	public async Task<IActionResult> GetAllInstructor(
		[FromQuery] SubmissionQuery query)
	{
		var userId = GetUserId();

		var dbQuery = _context.Submissions
			.Where(x =>
				(string.IsNullOrWhiteSpace(query.Title) || x.Title.Contains(query.Title)) &&
				(query.TopicIds.Length > 0 || (x.TopicId != null && query.TopicIds.Contains((int)x.TopicId))) &&
				(query.StudentUserId == null || query.StudentUserId == x.StudentUserId));

		var searchQuery = dbQuery
			.Skip((query.PageNumber - 1) * query.PageSize)
			.Take(query.PageSize);

		searchQuery = query.SortBy switch
		{
			SortBy.Title => searchQuery.OrderBy(x => x.Title),
			_ => searchQuery.OrderByDescending(x => x.Created),
		};

		var result = new SubmissionResults
		{
			Queried = await searchQuery
				.ProjectTo<SubmissionResults.Submission>(_mapper.ConfigurationProvider)
				.ToArrayAsync(),
			Total = await dbQuery
				.CountAsync()
		};

		return Ok(result);
	}

	// GET: /submissions/5
	[HttpGet("{submissionId}")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SubmissionDetailResult))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Get(
		int submissionId)
	{
		var userId = GetUserId();
		var isInstructor = IsInstructor();

		var result = await _context.Submissions
			.ProjectTo<SubmissionDetailResult>(_mapper.ConfigurationProvider)
			.FirstOrDefaultAsync(x =>
				x.SubmissionId == submissionId &&
				(isInstructor || x.StudentUserId == userId) &&
				DateTime.Now > x.Ended);
		if (result == null)
			return NotFound("SubmissionId does not exist" +
				" or hasn't ended yet" +
				" or you're not authorized.");

		return Ok(result);
	}

	// POST: /submissions/5/submit
	[HttpPost("{submissionId}/submit")]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Submit(
		int submissionId,
		[FromBody] SubmitSubmission body)
	{
		var userId = GetUserId();

		var now = DateTime.Now;
		var ongoing = await _context.Submissions
			.Include(x => x.Questions)
			.ThenInclude(x => x.Choices)
			.FirstOrDefaultAsync(x =>
				x.SubmissionId == submissionId &&
				x.StudentUserId == userId &&
				now < x.Deadline &&
				now < x.Ended);
		if (ongoing == null)
			return NotFound("SubmissionId does not exist" +
				" or has ended" +
				" or you're not authorized.");

		var manualGrading = ongoing.Questions.Any(x => x.Choices.Count <= 1);

		foreach (var answer in body.Answers)
		{
			var question = ongoing.Questions
				.FirstOrDefault(x => x.QuestionId == answer.QuestionId);
			if (question == null)
				return NotFound($"QuestionId ({answer.QuestionId}) does not exist.");

			if (answer.ChoiceId != null)
			{
				question.Choices.ForEach(x => x.IsChosen = false);
				var choice = question.Choices
					.FirstOrDefault(x => x.ChoiceId == answer.ChoiceId);
				if (choice == null)
					return NotFound($"ChoiceId ({answer.ChoiceId}) does not exist.");

				choice.IsChosen = true;
			}
			else
			{
				question.Choices.Clear();
				var choice = new Choice()
				{
					Content = answer.Content!
				};
				question.Choices.Add(choice);
			}
		}
		ongoing.Ended = now;

		if (!manualGrading)
		{
			var maxGrade = ongoing.Questions.Sum(x => x.Point);
			var quizGrade = ongoing.Questions
				.Where(x => x.Choices.Any(x => x.IsCorrect == x.IsChosen))
				.Sum(x => x.Point);

			ongoing.Grade = quizGrade;
			ongoing.Graded = now;

			if (quizGrade / (float)maxGrade > 0.75)
			{
				var completedUnit = new CompletedUnit()
				{
					EnrollmentId = ongoing.EnrollmentId,
					UnitId = ongoing.UnitId
				};
				_context.CompletedUnits.Add(completedUnit);
			}
		}

		await _context.SaveChangesAsync();

		return CreatedAtAction(nameof(Get), new { ongoing.SubmissionId }, null);
	}

	// POST: /submissions/5/grading
	[HttpPost("{submissionId}/grading")]
	[Authorize(Roles = nameof(RoleType.Instructor))]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Grading(
		int submissionId,
		[FromBody] GradingSubmission body)
	{
		var userId = GetUserId();

		var submission = await _context.Submissions
			.Include(x => x.Questions)
			.ThenInclude(x => x.Choices)
			.Include(x => x.Reviews)
			.FirstOrDefaultAsync(x =>
				x.SubmissionId == submissionId &&
				x.Graded != null);
		if (submission == null)
			return NotFound("SubmissionId does not exist or has graded.");

		var maxManualGrade = submission.Questions
			.Where(x => x.Choices.Count <= 1)
			.Sum(x => x.Point);
		if (body.ManualGrade > maxManualGrade)
			return BadRequest($"Maximum manual grade is ({maxManualGrade})");

		var reviews = _mapper.Map<IEnumerable<Review>>(body.Reviews);
		submission.Reviews.AddRange(reviews);

		var maxGrade = submission.Questions.Sum(x => x.Point);
		var quizGrade = submission.Questions
			.Where(x => x.Choices.Any(x => x.IsCorrect == x.IsChosen))
			.Sum(x => x.Point);

		submission.Grade = quizGrade + body.ManualGrade;
		submission.Graded = DateTime.Now;
		submission.InstructorUserId = userId;

		if (quizGrade / (float)maxGrade > 0.75)
		{
			var completedUnit = new CompletedUnit()
			{
				EnrollmentId = submission.EnrollmentId,
				UnitId = submission.UnitId
			};
			_context.CompletedUnits.Add(completedUnit);
		}

		await _context.SaveChangesAsync();

		return CreatedAtAction(nameof(Get), new { submissionId }, null);
	}


	// POST: /submissions/5/comment
	[HttpPost("{submissionId}/comment")]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Comment(
		int submissionId,
		[FromBody] string commentContent)
	{
		var userId = GetUserId();
		var isInstructor = IsInstructor();

		var submission = await _context.Submissions
			.FirstOrDefaultAsync(x =>
				x.SubmissionId == submissionId &&
				x.Graded == null &&
				(x.InstructorUserId == userId || x.StudentUserId == userId));
		if (submission == null)
			return NotFound("SubmissionId does not exist" +
				" or hasn't graded yet" +
				" or you're not authorized.");

		var comment = new Comment()
		{
			Content = commentContent,
			Created = DateTime.Now,
			OwnerUserId = userId,
			SubmissionId = submissionId
		};
		_context.Comments.Add(comment);

		if (isInstructor)
			submission.InstructorUserId ??= userId;

		await _context.SaveChangesAsync();

		return CreatedAtAction(nameof(Get), new { submissionId }, null);
	}


	private int GetUserId()
	{
		return ClaimUtilities.GetUserId(User.Claims);
	}

	private bool IsInstructor()
	{
		return User.IsInRole(nameof(RoleType.Instructor));
	}
}
