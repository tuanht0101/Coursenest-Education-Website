namespace UserData.API.Infrastructure.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class Comment
{
	public int CommentId { get; set; }
	public string Content { get; set; }
	public DateTime Created { get; set; }

	// Relationship
	public int OwnerUserId { get; set; }
	public int SubmissionId { get; set; }
	public Submission Submission { get; set; }
}
