namespace UserData.API.Infrastructure.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class Review
{
	public int ReviewId { get; set; }
	public string Content { get; set; }
	public ReviewType Type { get; set; }

	// Relationship
	public int SubmissionId { get; set; }
	public Submission Submission { get; set; }

	public enum ReviewType
	{
		Negative, Attentive, Informative, Positive
	}
}
