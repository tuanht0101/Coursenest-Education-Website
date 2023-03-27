namespace Library.API.DTOs.Units;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record CreateUnit
{
	public string Title { get; set; }
	public int RequiredMinutes { get; set; }
	public int LessonId { get; set; }
}

public record CreateExam : CreateUnit
{
}

public record CreateMaterial : CreateUnit
{
	public string Content { get; set; }
}