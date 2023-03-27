namespace Library.API.Infrastructure.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class Lesson
{
	public int LessonId { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
	public int OrderNumerator { get; set; }
	public int OrderDenominator { get; set; }
	public double Order { get; set; }

	// Relationship
	public int CourseId { get; set; }
	public Course Course { get; set; }
	public List<Unit> Units { get; set; }
}
