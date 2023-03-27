namespace Authentication.API.DTOs;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record ForgotPassword
{
	public string Username { get; set; }
	public string Email { get; set; }
}
