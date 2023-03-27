namespace Authentication.API.DTOs;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

// TODO [DTOs.Login] Add validation
public record Login
{
	public string Username { get; init; }
	public string Password { get; init; }
}
