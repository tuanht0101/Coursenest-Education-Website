namespace Library.API.DTOs.Units;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record UpdateQuestion
{
	public string? Content { get; set; }
	public int? Point { get; set; }
	public int ExamUnitId { get; set; }
	public List<CreateQuestion.CreateChoice>? Choices { get; set; }
}
