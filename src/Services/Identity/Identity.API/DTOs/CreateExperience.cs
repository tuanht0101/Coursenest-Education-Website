namespace Identity.API.DTOs;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record CreateExperience
{
	public string Name { get; set; }
	public string Title { get; set; }
	public DateTime Started { get; set; }
	public DateTime? Ended { get; set; }
}
