using Authentication.API.DTOs;
using Authentication.API.Infrastructure.Contexts;
using Authentication.API.Infrastructure.Entities;
using Authentication.API.Options;
using Authentication.API.Utilities;
using AutoMapper;
using CommonLibrary.API.MessageBus.Commands;
using CommonLibrary.API.MessageBus.Responses;
using CommonLibrary.API.Models;
using CommonLibrary.API.Utilities.APIs;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Data;
using System.Security.Claims;

namespace Authentication.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticateController : ControllerBase
{
	private readonly IMapper _mapper;
	private readonly DataContext _context;
	private readonly IOptionsMonitor<JwtOptions> _jwtOptions;
	private readonly JwtTokenHelper _jwtTokenHelper;
	private readonly IRequestClient<CreateUser> _createUserClient;
	private readonly IRequestClient<CheckTopics> _checkTopicsClient;
	private readonly IRequestClient<CheckUsers> _checkUsersClient;

	public AuthenticateController(
		IMapper mapper,
		DataContext context,
		IOptionsMonitor<JwtOptions> jwtOptions,
		JwtTokenHelper jwtTokenHelper,
		IRequestClient<CreateUser> createUserClient,
		IRequestClient<CheckTopics> checkTopicsClient,
		IRequestClient<CheckUsers> checkUsersClient)
	{
		_mapper = mapper;
		_context = context;
		_jwtOptions = jwtOptions;
		_jwtTokenHelper = jwtTokenHelper;
		_createUserClient = createUserClient;
		_checkTopicsClient = checkTopicsClient;
		_checkUsersClient = checkUsersClient;
	}

	// POST: /authenticate/register
	[HttpPost("register")]
	[AllowAnonymous]
	public async Task<ActionResult> Register(
		[FromBody] Register body)
	{
		var exists = await _context.Credentials
			.AnyAsync(x => x.Username == body.Username);
		if (exists)
			return Conflict("Username existed.");

		var checkTopicsRequest = new CheckTopics()
		{
			Ids = body.InterestedTopicIds
		};
		var checkTopicsResponse = await _checkTopicsClient
			.GetResponse<Existed, NotFound>(checkTopicsRequest);
		if (checkTopicsResponse.Is(out Response<NotFound>? notFoundResponse))
		{
			return NotFound(notFoundResponse!.Message.Message);
		}

		var createUserRequest = _mapper.Map<CreateUser>(body);
		var createUserResponse = await _createUserClient
			.GetResponse<Created, Existed>(createUserRequest);
		if (createUserResponse.Is(out Response<Existed>? existedResponse))
		{
			return Conflict(existedResponse!.Message);
		}
		if (!createUserResponse.Is(out Response<Created>? createdResponse) ||
			createdResponse == null)
		{
			return StatusCode(StatusCodes.Status500InternalServerError);
		}

		var credential = _mapper.Map<Credential>(body);
		credential.UserId = createdResponse.Message.Id;

		_context.Credentials.Add(credential);
		await _context.SaveChangesAsync();

		return CreatedAtAction(nameof(Login), null, null);
	}

	// POST: /authenticate/login
	[HttpPost("login")]
	[AllowAnonymous]
	public async Task<ActionResult<TokensResult>> Login(
		[FromBody] Login body)
	{
		var credential = await _context.Credentials
			.AsNoTracking()
			.Include(x => x.Roles)
			.FirstOrDefaultAsync(x =>
				x.Username == body.Username &&
				x.Password == body.Password);
		if (credential == null)
			return BadRequest("Credential is invalid.");

		(string accessTokenContent, DateTime accessTokenExpiry) = CreateAccessToken(credential.UserId, credential.Roles);

		string refreshTokenContent = Guid.NewGuid().ToString();
		var refreshTokenExpiry = DateTime.Now.AddMinutes(_jwtOptions.CurrentValue.RefreshTokenLifetime);
		var refreshToken = new RefreshToken()
		{
			Token = refreshTokenContent,
			Expiry = refreshTokenExpiry,
			CredentialUserId = credential.UserId
		};
		_context.RefreshTokens.Add(refreshToken);

		await _context.SaveChangesAsync();

		var result = new TokensResult()
		{
			UserId = credential.UserId,
			AccessToken = accessTokenContent,
			AccessTokenExpiry = accessTokenExpiry,
			RefreshToken = refreshTokenContent,
			RefreshTokenExpiry = refreshTokenExpiry
		};

		return result;
	}

	// POST: /authenticate/logout
	[HttpPost("logout")]
	[Authorize]
	public async Task<ActionResult> Logout()
	{
		var userId = GetUserId();

		var affected = await _context.RefreshTokens
			.Where(x => x.CredentialUserId == userId)
			.ExecuteDeleteAsync();
		if (affected == 0) return NotFound();

		return Ok();
	}

	// POST: /authenticate/refresh
	[HttpPost("refresh")]
	[AllowAnonymous]
	public async Task<ActionResult<AccessTokenResult>> Refresh(
		[FromBody] string token)
	{
		var refreshToken = await _context.RefreshTokens
			.Include(x => x.Credential.Roles)
			.FirstOrDefaultAsync(x => x.Token == token);
		if (refreshToken == null)
			return NotFound("RefreshToken does not exist.");

		if (refreshToken.Expiry <= DateTime.Now)
		{
			_context.RefreshTokens.Remove(refreshToken);

			await _context.SaveChangesAsync();
			return BadRequest("RefreshToken expired.");
		}

		(string content, DateTime expiry) = CreateAccessToken(refreshToken.CredentialUserId, refreshToken.Credential.Roles);

		var result = new AccessTokenResult()
		{
			AccessToken = content,
			AccessTokenExpiry = expiry
		};

		return result;
	}

	// PUT: /authenticate/reset-password
	[HttpPut("reset-password")]
	[Authorize(Roles = nameof(RoleType.Admin))]
	public async Task<ActionResult<string>> ResetPassword(
		[FromBody] int userId)
	{
		var credential = await _context.Credentials
			.FirstOrDefaultAsync(x => x.UserId == userId);
		if (credential == null)
			return NotFound($"UserId: {userId} does not exist.");

		var newPassword = Guid.NewGuid().ToString("N")[..8];
		credential.Password = newPassword;

		await _context.SaveChangesAsync();

		return newPassword;
	}

	// PUT: /authenticate/forgot-password
	[HttpPut("forgot-password")]
	[AllowAnonymous]
	public async Task<ActionResult<string>> ForgotPassword(
		[FromBody] ForgotPassword body)
	{
		var credential = await _context.Credentials
			.FirstOrDefaultAsync(x => x.Username == body.Username);
		if (credential == null)
			return NotFound("Credential does not exist.");

		var query = new CheckUsers.Query()
		{
			Id = credential.UserId,
			Email = body.Email
		};
		var request = new CheckUsers()
		{
			Queries = new[] { query }
		};
		var response = await _checkUsersClient
			.GetResponse<Existed, NotFound>(request);
		if (response.Is(out Response<NotFound>? notFoundResponse))
		{
			return NotFound(notFoundResponse!.Message.Message);
		}

		var newPassword = Guid.NewGuid().ToString("N")[..8];
		credential.Password = newPassword;

		await _context.SaveChangesAsync();

		return newPassword;
	}

	// PUT: /authenticate/change-password
	[HttpPut("change-password")]
	[Authorize]
	public async Task<ActionResult> ChangePassword(
		[FromBody] ChangePassword body)
	{
		var userId = GetUserId();

		var credential = await _context.Credentials
			.FirstOrDefaultAsync(x => x.UserId == userId);
		if (credential == null)
			return NotFound($"UserId: {userId} does not exist.");
		if (credential.Password != body.OldPassword)
			return BadRequest("Old password is invalid.");

		credential.Password = body.NewPassword;
		await _context.SaveChangesAsync();

		return NoContent();
	}


	private (string, DateTime) CreateAccessToken(
		int userId,
		IEnumerable<Role> roles)
	{
		var claims = new List<Claim>
		{
			new Claim(ClaimTypes.NameIdentifier, userId.ToString())
		};

		var validRoles = roles
			.Where(x => x.Expiry > DateTime.Now.AddMinutes(5));

		var roleClaims = validRoles
			.Select(x => new Claim(ClaimTypes.Role, x.Type.ToString()));
		claims.AddRange(roleClaims);

		var expiry = validRoles
			.Select(x => x.Expiry)
			.Append(DateTime.Now.AddMinutes(_jwtOptions.CurrentValue.AccessTokenLifetime))
			.Min();

		var token = _jwtTokenHelper.CreateToken(claims, expiry);

		return (token, expiry);
	}

	private int GetUserId()
	{
		return ClaimUtilities.GetUserId(User.Claims);
	}
}
