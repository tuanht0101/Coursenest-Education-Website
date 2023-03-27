namespace CommonLibrary.API.MessageBus.Responses;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record ExamResult
{
	public int UnitId { get; set; }
	public string Title { get; set; }
	public int TimeLimitInMinutes { get; set; }
	public string LessonTitle { get; set; }
	public string CourseTitle { get; set; }
	public int? TopicId { get; set; }
	public List<Question> Questions { get; set; }

	public record Question
	{
		public string Content { get; set; }
		public byte Point { get; set; }
		public List<Choice> Choices { get; set; }
	}

	public record Choice
	{
		public string Content { get; set; }
		public bool IsCorrect { get; set; }
	}
}
