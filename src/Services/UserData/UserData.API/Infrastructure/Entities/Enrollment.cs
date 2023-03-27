namespace UserData.API.Infrastructure.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class Enrollment
{
	public int EnrollmentId { get; set; }
	public DateTime? Completed { get; set; }

	// Relationship
	public int CourseId { get; set; }
	public int StudentUserId { get; set; }
	public List<CompletedUnit> CompletedUnits { get; set; }
	public List<Submission> Submissions { get; set; }
}
