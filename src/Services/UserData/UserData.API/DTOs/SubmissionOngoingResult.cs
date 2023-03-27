namespace UserData.API.DTOs;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record SubmissionOngoingResult
{
	public int SubmissionId { get; set; }
	public string Title { get; set; }
	public string LessonTitle { get; set; }
	public string CourseTitle { get; set; }
	public DateTime Created { get; set; }
	public DateTime Deadline { get; set; }

	// Relationship
	public int StudentUserId { get; set; }
	public int UnitId { get; set; }
	public int EnrollmentId { get; set; }
	public List<QuestionOngoingResult> Questions { get; set; }

	public record QuestionOngoingResult
	{
		public int QuestionId { get; set; }
		public string Content { get; set; }
		public int Point { get; set; }
		public List<ChoiceOngoingResult> Choices { get; set; }
	}

	public record ChoiceOngoingResult
	{
		public int ChoiceId { get; set; }
		public string Content { get; set; }
	}
}
