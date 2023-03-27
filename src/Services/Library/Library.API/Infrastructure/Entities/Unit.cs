namespace Library.API.Infrastructure.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public abstract class Unit
{
	public int UnitId { get; set; }
	public string Title { get; set; }
	public int RequiredMinutes { get; set; }
	public int OrderNumerator { get; set; }
	public int OrderDenominator { get; set; }
	public double Order { get; set; }

	// Relationship
	public int LessonId { get; set; }
	public Lesson Lesson { get; set; }
}
