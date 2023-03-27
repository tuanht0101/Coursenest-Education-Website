namespace Library.API.DTOs.Lessons;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record LessonResult
{
	public int LessonId { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
	public double Order { get; set; }
}
