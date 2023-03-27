namespace CommonLibrary.API.MessageBus.Commands;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record CreateUserAchievement
{
	public string Title { get; set; }
	public DateTime Created { get; set; }
	public int UserId { get; set; }
}
