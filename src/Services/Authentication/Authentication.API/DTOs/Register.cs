namespace Authentication.API.DTOs;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

// TODO [DTOs.Register] Add validation
public record Register
{
	public string Username { get; set; }
	public string Password { get; set; }
	public string Email { get; set; }
	public string Fullname { get; set; }
	public int[] InterestedTopicIds { get; set; }
}
