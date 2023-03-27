namespace UserData.API.Infrastructure.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class Submission
{
	public int SubmissionId { get; set; }
	public string Title { get; set; }
	public string LessonTitle { get; set; }
	public string CourseTitle { get; set; }
	public DateTime Created { get; set; }
	public DateTime Deadline { get; set; }
	public DateTime Ended { get; set; }
	public int? Grade { get; set; }
	public DateTime? Graded { get; set; }

	// Relationship
	public int StudentUserId { get; set; }
	public int? InstructorUserId { get; set; }
	public int UnitId { get; set; }
	public int EnrollmentId { get; set; }
	public int? TopicId { get; set; }
	public Enrollment Enrollment { get; set; }
	public List<Question> Questions { get; set; }
	public List<Review> Reviews { get; set; }
	public List<Comment> Comments { get; set; }
}
