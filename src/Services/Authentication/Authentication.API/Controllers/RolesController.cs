using Authentication.API.DTOs;
using Authentication.API.Infrastructure.Contexts;
using Authentication.API.Infrastructure.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CommonLibrary.API.Models;
using CommonLibrary.API.Utilities.APIs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Authentication.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
	private readonly IMapper _mapper;
	private readonly DataContext _context;

	public RolesController(IMapper mapper, DataContext context)
	{
		_mapper = mapper;
		_context = context;
	}


	// GET: /roles/5
	[HttpGet("{userId}")]
	[Authorize(Roles = nameof(RoleType.Admin))]
	public async Task<ActionResult<IEnumerable<RoleResult>>> GetAll(
		int userId)
	{
		var exists = await _context.Credentials
			.AnyAsync(x => x.UserId == userId);
		if (!exists)
			return NotFound($"UserId: {userId} does not exist.");

		var results = await _context.Roles
			.Where(x => x.CredentialUserId == userId)
			.ProjectTo<RoleResult>(_mapper.ConfigurationProvider)
			.ToListAsync();

		return results;
	}

	// GET: /roles/me
	[HttpGet("me")]
	[Authorize]
	public async Task<ActionResult<IEnumerable<RoleResult>>> GetAllMe()
	{
		var userId = GetUserId();

		var exists = await _context.Credentials
			.AnyAsync(x => x.UserId == userId);
		if (!exists)
			return NotFound($"UserId: {userId} does not exist.");

		var results = await _context.Roles
			.Where(x => x.CredentialUserId == userId)
			.ProjectTo<RoleResult>(_mapper.ConfigurationProvider)
			.ToListAsync();

		return results;
	}


	// PUT: /roles/5
	[HttpPut("{userId}")]
	[Authorize(Roles = nameof(RoleType.Admin))]
	public async Task<ActionResult> Update(
		int userId,
		[FromBody] SetRole body)
	{
		var credential = await _context.Credentials
			.Include(x => x.Roles)
			.FirstOrDefaultAsync(x => x.UserId == userId);
		if (credential == null)
			return NotFound($"UserId: {userId} does not exist.");

		var role = credential.Roles
			.FirstOrDefault(x => x.Type == body.Type);
		if (role == null)
		{
			role = _mapper.Map<Role>(body);
			credential.Roles.Add(role);
		}
		else
		{
			_mapper.Map(body, role);
		}

		await _context.SaveChangesAsync();

		return CreatedAtAction(nameof(GetAll), new { userId }, null);
	}


	private int GetUserId()
	{
		return ClaimUtilities.GetUserId(User.Claims);
	}
}
