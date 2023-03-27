namespace CommonLibrary.API.MessageBus.Commands;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record CreateUser
{
	public string Email { get; set; }
	public string FullName { get; set; }
	public int[] InterestedTopicIds { get; set; }
}
