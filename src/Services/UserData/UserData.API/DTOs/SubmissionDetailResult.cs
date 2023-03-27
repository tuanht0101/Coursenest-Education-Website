using static UserData.API.Infrastructure.Entities.Review;

namespace UserData.API.DTOs;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record SubmissionDetailResult
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
	public int StudentUserId { get; set; }
	public int? InstructorUserId { get; set; }
	public int UnitId { get; set; }
	public int EnrollmentId { get; set; }
	public int? TopicId { get; set; }
	public List<QuestionDetailResult> Questions { get; set; }
	public List<ReviewDetailResult> Reviews { get; set; }
	public List<CommentDetailResult> Comments { get; set; }


	public record QuestionDetailResult
	{
		public int QuestionId { get; set; }
		public string Content { get; set; }
		public int Point { get; set; }
		public List<ChoiceDetailResult> Choices { get; set; }
	}

	public record ChoiceDetailResult
	{
		public int ChoiceId { get; set; }
		public string Content { get; set; }
		public bool IsCorrect { get; set; }
		public bool? IsChosen { get; set; }
	}

	public record ReviewDetailResult
	{
		public int ReviewId { get; set; }
		public string Content { get; set; }
		public ReviewType Type { get; set; }
	}

	public record CommentDetailResult
	{
		public int CommentId { get; set; }
		public string Content { get; set; }
		public DateTime Created { get; set; }
		public int OwnerUserId { get; set; }
	}
}
