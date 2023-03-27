namespace UserData.API.Infrastructure.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class Choice
{
	public int ChoiceId { get; set; }
	public string Content { get; set; }
	public bool IsCorrect { get; set; }
	public bool? IsChosen { get; set; }

	// Relationship
	public int QuestionId { get; set; }
	public Question Question { get; set; }
}
