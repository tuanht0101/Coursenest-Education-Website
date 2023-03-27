namespace Library.API.DTOs.Units;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record UnitResult
{
	public int UnitId { get; set; }
	public int CourseId { get; set; }
	public string Title { get; set; }
	public int RequiredMinutes { get; set; }
	public double Order { get; set; }
	public bool IsExam { get; set; }
}

public record ExamResult : UnitResult
{
	public List<QuestionResult> Questions { get; set; }
}

public record MaterialResult : UnitResult
{
	public string Content { get; set; }
	public int PublisherUserId { get; set; }
}
