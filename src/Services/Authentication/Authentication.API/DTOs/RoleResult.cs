using CommonLibrary.API.Models;

namespace Authentication.API.DTOs;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record RoleResult
{
	public RoleType Type { get; set; }
	public DateTime Expiry { get; set; }
}
