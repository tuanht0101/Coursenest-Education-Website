using static UserData.API.Infrastructure.Entities.Review;

namespace UserData.API.DTOs;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record GradingSubmission
{
	public Review[] Reviews { get; set; }
	public int ManualGrade { get; set; }

	public record Review
	{
		public string Content { get; set; }
		public ReviewType Type { get; set; }
	}
}
