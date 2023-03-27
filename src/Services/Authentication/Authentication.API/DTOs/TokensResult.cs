namespace Authentication.API.DTOs;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record TokensResult
{
	public int UserId { get; set; }
	public string AccessToken { get; set; }
	public DateTime AccessTokenExpiry { get; set; }
	public string RefreshToken { get; set; }
	public DateTime RefreshTokenExpiry { get; set; }
}
