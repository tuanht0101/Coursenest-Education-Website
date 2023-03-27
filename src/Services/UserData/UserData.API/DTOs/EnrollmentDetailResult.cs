namespace UserData.API.DTOs;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record EnrollmentDetailResult
{
	public int EnrollmentId { get; set; }
	public DateTime? Completed { get; set; }
	public int CourseId { get; set; }
	public int StudentUserId { get; set; }
	public IEnumerable<int> CompletedUnitIds { get; set; }
}
