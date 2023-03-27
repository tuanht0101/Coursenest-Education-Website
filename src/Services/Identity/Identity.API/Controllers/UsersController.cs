using AutoMapper;
using AutoMapper.QueryableExtensions;
using CommonLibrary.API.Constants;
using CommonLibrary.API.MessageBus.Commands;
using CommonLibrary.API.MessageBus.Events;
using CommonLibrary.API.MessageBus.Responses;
using CommonLibrary.API.Models;
using CommonLibrary.API.Utilities.APIs;
using CommonLibrary.API.Validations;
using Identity.API.DTOs;
using Identity.API.Infrastructure.Contexts;
using Identity.API.Infrastructure.Entities;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using static Identity.API.DTOs.UserAdminResults;

namespace Identity.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly DataContext _context;
		private readonly IRequestClient<CheckTopics> _checkTopicsClient;
		private readonly IRequestClient<GetCredentials> _getCredentialsClient;
		private readonly IPublishEndpoint _publishEndpoint;

		public UsersController(
			IMapper mapper,
			DataContext context,
			IRequestClient<CheckTopics> checkTopicsClient,
			IRequestClient<GetCredentials> getCredentialsClient,
			IPublishEndpoint publishEndpoint)
		{
			_mapper = mapper;
			_context = context;
			_checkTopicsClient = checkTopicsClient;
			_getCredentialsClient = getCredentialsClient;
			_publishEndpoint = publishEndpoint;
		}


		// GET: /users/admin
		[HttpGet("admin")]
		[Authorize(Roles = nameof(RoleType.Admin))]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserAdminResults))]
		public async Task<IActionResult> GetAllAdmin(
			[FromQuery] UserQuery query)
		{
			var dbQuery = _context.Users
				.Where(x =>
					string.IsNullOrWhiteSpace(query.FullName) ||
					x.FullName.Contains(query.FullName));

			var count = await dbQuery
				.CountAsync();

			if (count == 0)
			{
				return Ok(new UserAdminResults()
				{
					Queried = Array.Empty<UserAdminResult>(),
					Count = 0
				});
			}

			var users = await dbQuery
				.OrderBy(x => x.FullName)
				.Skip((query.PageNumber - 1) * query.PageSize)
				.Take(query.PageSize)
				.ProjectTo<UserAdminResult>(_mapper.ConfigurationProvider)
				.ToArrayAsync();

			var request = new GetCredentials()
			{
				Ids = users.Select(x => x.UserId).ToArray()
			};
			var response = await _getCredentialsClient
				.GetResponse<CredentialResults>(request);
			if (response == null)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}

			foreach (var user in users)
			{
				var cred = response.Message.Credentials
					.FirstOrDefault(x => x.UserId == user.UserId);
				_mapper.Map(cred, user);
			}

			return Ok(new UserAdminResults()
			{
				Queried = users,
				Count = count
			});
		}

		// GET: /users/admin/count
		[HttpGet("admin/count")]
		[Authorize(Roles = nameof(RoleType.Admin))]
		public async Task<ActionResult<int>> GetAdminCount(
			[FromQuery] string? fullName)
		{
			var result = await _context.Users
				.Where(x =>
					string.IsNullOrWhiteSpace(fullName) ||
					x.FullName.Contains(fullName))
				.CountAsync();

			return result;
		}

		// DELETE: /users/5
		[HttpDelete("{userId}")]
		[Authorize(Roles = nameof(RoleType.Admin))]
		public async Task<ActionResult> Delete(
			int userId)
		{
			var affected = await _context.Users
				.Where(x => x.UserId == userId)
				.ExecuteDeleteAsync();
			if (affected == 0)
				return NotFound($"UserId: {userId} does not exist.");

			var request = new UserDeleted() { UserId = userId };
			await _publishEndpoint.Publish(request);

			return NoContent();
		}


		// GET: /users
		[HttpGet]
		[AllowAnonymous]
		public async Task<ActionResult<IEnumerable<UserResult>>> GetAll(
			[FromQuery] int[] ids)
		{
			var results = await _context.Users
				.Where(x => ids.Contains(x.UserId))
				.ProjectTo<UserResult>(_mapper.ConfigurationProvider)
				.ToListAsync();

			return results;
		}

		// GET: /users/5
		[HttpGet("{userId}")]
		[AllowAnonymous]
		public async Task<ActionResult<UserProfileResult>> Get(
			int userId)
		{
			var result = await _context.Users
				.ProjectTo<UserProfileResult>(_mapper.ConfigurationProvider)
				.FirstOrDefaultAsync(x => x.UserId == userId);
			if (result == null)
				return NotFound($"UserId: {userId} does not exist.");

			return result;
		}

		// GET: /users/5/instructor
		[HttpGet("{userId}/instructor")]
		[AllowAnonymous]
		public async Task<ActionResult<UserInstructorResult>> GetInstructor(
			int userId)
		{
			var result = await _context.Users
				.ProjectTo<UserInstructorResult>(_mapper.ConfigurationProvider)
				.FirstOrDefaultAsync(x => x.UserId == userId);
			if (result == null)
				return NotFound($"UserId: {userId} does not exist.");

			return result;
		}


		// PUT: /users/me
		[HttpPut("me")]
		[Authorize]
		public async Task<ActionResult> Update(
			[FromBody] UpdateUser body)
		{
			var userId = GetUserId();

			var user = await _context.Users
				.FirstOrDefaultAsync(x => x.UserId == userId);
			if (user == null)
				return NotFound($"UserId: {userId} does not exist.");

			var emailExists = await _context.Users
				.AnyAsync(x => x.Email == body.Email);
			if (emailExists)
				return Conflict("Email existed.");

			_mapper.Map(body, user);

			await _context.SaveChangesAsync();

			return NoContent();
		}

		// PUT: /users/me/cover
		[HttpPut("me/cover")]
		[Authorize]
		public async Task<ActionResult> UpdateCover(
			[BindRequired][MaxSize(0, 2 * 1024 * 1024)][ImageExtension] IFormFile formFile)
		{
			var userId = GetUserId();

			var user = await _context.Users
				.Include(x => x.Avatar)
				.FirstOrDefaultAsync(x => x.UserId == userId);
			if (user == null)
				return NotFound($"UserId: {userId} does not exist.");

			using var memoryStream = new MemoryStream();
			await formFile.CopyToAsync(memoryStream);
			var extension = Path.GetExtension(formFile.FileName).ToLowerInvariant();

			user.Avatar = new Avatar()
			{
				MediaType = FormFileContants.ExtensionMIMEs.GetValueOrDefault(extension)!,
				Data = memoryStream.ToArray(),
				UserId = userId,
			};
			user.LastModified = DateTime.Now;

			await _context.SaveChangesAsync();

			return NoContent();
		}


		// POST: /users/me/interest
		[HttpPost("me/interest")]
		[Authorize]
		public async Task<ActionResult> AddInterestedTopic(
			[FromBody] int topicId)
		{
			var userId = GetUserId();

			var user = await _context.Users
				.Select(x =>
					new
					{
						x.UserId,
						InterestedTopicIds = x.InterestedTopics.Select(x => x.TopicId)
					})
				.FirstOrDefaultAsync(x => x.UserId == userId);
			if (user == null)
				return NotFound($"UserId: {userId} does not exist.");
			if (user.InterestedTopicIds.Contains(topicId))
				return Conflict($"InterestedTopicId: {topicId} existed.");

			var request = new CheckTopics()
			{
				Ids = new[] { topicId }
			};
			var response = await _checkTopicsClient
				.GetResponse<Existed, NotFound>(request);

			if (response.Is(out Response<NotFound>? notFoundResponse))
			{
				return NotFound(notFoundResponse!.Message.Message);
			}

			var topic = new InterestedTopic()
			{
				UserId = userId,
				TopicId = topicId
			};
			_context.InterestedTopics.Add(topic);

			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(Get), new { userId }, null);
		}

		// DELETE: /users/me/interest/5
		[HttpDelete("me/interest/{topicId}")]
		[Authorize]
		public async Task<ActionResult> DeleteInterestedTopic(
			int topicId)
		{
			var userId = GetUserId();

			var result = await _context.InterestedTopics
				.Where(x => x.UserId == userId && x.TopicId == topicId)
				.ExecuteDeleteAsync();
			if (result == 0)
				return NotFound($"InterestedTopicId: {topicId} does not exists.");

			return NoContent();
		}


		// GET: /users/me/follow
		[HttpGet("me/follow")]
		[Authorize]
		public async Task<ActionResult<IEnumerable<int>>> GetAllFollowedTopic()
		{
			var userId = GetUserId();

			var results = await _context.Users
				.Where(x => x.UserId == userId)
				.Select(x => x.FollowedTopics.Select(x => x.TopicId))
				.FirstOrDefaultAsync();
			if (results == null)
				return NotFound($"UserId: {userId} does not exist.");

			return Ok(results);
		}

		// POST: /users/me/follow
		[HttpPost("me/follow")]
		[Authorize]
		public async Task<ActionResult> AddFollowedTopic(
			[FromBody] int topicId)
		{
			var userId = GetUserId();

			var user = await _context.Users
				.Select(x =>
					new
					{
						x.UserId,
						FollowedTopicIds = x.FollowedTopics.Select(x => x.TopicId)
					})
				.FirstOrDefaultAsync(x => x.UserId == userId);
			if (user == null)
				return NotFound($"UserId: {userId} does not exist.");
			if (user.FollowedTopicIds.Contains(topicId))
				return Conflict($"FollowedTopicId: {topicId} existed.");

			var request = new CheckTopics()
			{
				Ids = new[] { topicId }
			};
			var response = await _checkTopicsClient
				.GetResponse<Existed, NotFound>(request);

			if (response.Is(out Response<NotFound>? notFoundResponse))
			{
				return NotFound(notFoundResponse!.Message.Message);
			}

			var topic = new FollowedTopic()
			{
				UserId = userId,
				TopicId = topicId
			};
			_context.FollowedTopics.Add(topic);

			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(Get), new { userId }, null);
		}

		// DELETE: /users/me/follow/5
		[HttpDelete("me/follow/{topicId}")]
		[Authorize]
		public async Task<ActionResult> DeleteFollowedTopic(
			int topicId)
		{
			var userId = GetUserId();

			var result = await _context.FollowedTopics
				.Where(x => x.UserId == userId && x.TopicId == topicId)
				.ExecuteDeleteAsync();
			if (result == 0)
				return NotFound($"FollowedTopicId: {topicId} does not exists.");

			return NoContent();
		}


		// POST: /users/me/experiences
		[HttpPost("me/experiences")]
		[Authorize]
		public async Task<ActionResult> AddExperience(
			[FromBody] CreateExperience body)
		{
			var userId = GetUserId();

			var user = await _context.Users
				.Include(x => x.Experiences)
				.FirstOrDefaultAsync(x => x.UserId == userId);
			if (user == null)
				return NotFound($"UserId: {userId} does not exist.");

			var experience = _mapper.Map<Experience>(body);
			user.Experiences.Add(experience);
			user.LastModified = DateTime.Now;

			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(Get), new { userId }, null);
		}

		// DELETE: /users/me/experiences/2
		[HttpDelete("me/experiences/{experienceId}")]
		[Authorize]
		public async Task<ActionResult> DeleteExperience(
			int experienceId)
		{
			var userId = GetUserId();

			var affected = await _context.Experiences
				.Where(x => x.UserId == userId && x.ExperienceId == experienceId)
				.ExecuteDeleteAsync();
			if (affected == 0)
				return NotFound($"ExperienceId: {experienceId} does not exist.");

			return NoContent();
		}


		private int GetUserId()
		{
			return ClaimUtilities.GetUserId(User.Claims);
		}
	}
}
