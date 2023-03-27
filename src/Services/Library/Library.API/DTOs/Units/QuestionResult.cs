namespace Library.API.DTOs.Units;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record QuestionResult
{
	public int QuestionId { get; set; }
	public string Content { get; set; }
	public int Point { get; set; }
	public List<ChoiceResult> Choices { get; set; }
}
