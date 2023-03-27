namespace Library.API.Infrastructure.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class Exam : Unit
{
	// Relationship
	public List<Question> Questions { get; set; }
}
