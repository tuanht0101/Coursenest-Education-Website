namespace Identity.API.DTOs;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record UserResult
{
	public int UserId { get; set; }
	public string FullName { get; set; }
	public ImageResult? Avatar { get; set; }
}
