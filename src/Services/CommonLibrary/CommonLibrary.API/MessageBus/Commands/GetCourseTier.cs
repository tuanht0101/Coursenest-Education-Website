namespace CommonLibrary.API.MessageBus.Commands;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record GetCourseTier
{
	public int Id { get; set; }
	public bool? IsApproved { get; set; }
}
