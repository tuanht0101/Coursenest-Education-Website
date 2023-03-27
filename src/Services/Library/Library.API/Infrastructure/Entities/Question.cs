namespace Library.API.Infrastructure.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class Question
{
	public int QuestionId { get; set; }
	public string Content { get; set; }
	public int Point { get; set; }

	// Relationship
	public int ExamUnitId { get; set; }
	public Exam Exam { get; set; }
	public List<Choice> Choices { get; set; }
}
