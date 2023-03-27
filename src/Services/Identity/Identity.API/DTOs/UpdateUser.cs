using Identity.API.Infrastructure.Entities;

namespace Identity.API.DTOs;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record UpdateUser
{
	public string? Email { get; set; }
	public string? Phonenumber { get; set; }
	public string? FullName { get; set; }
	public string? Title { get; set; }
	public string? AboutMe { get; set; }
	public Gender? Gender { get; set; }
	public DateTime? DateOfBirth { get; set; }
	public string? Location { get; set; }
}
