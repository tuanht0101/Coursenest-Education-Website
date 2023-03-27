namespace UserData.API.DTOs;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record SubmissionResults
{
	public Submission[] Queried { get; set; }
	public int Total { get; set; }

	public record Submission
	{
		public int SubmissionId { get; set; }
		public string Title { get; set; }
		public string LessonTitle { get; set; }
		public string CourseTitle { get; set; }
		public DateTime Created { get; set; }
		public int? Grade { get; set; }
		public DateTime? Graded { get; set; }
		public int UnitId { get; set; }
		public int? TopicId { get; set; }
	}
}
