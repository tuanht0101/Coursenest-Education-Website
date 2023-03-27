namespace CommonLibrary.API.MessageBus.Commands;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record CheckUsers
{
	public Query[] Queries { get; set; }

	public record Query
	{
		public int? Id { get; set; }
		public string? Email { get; set; }
	}
}
