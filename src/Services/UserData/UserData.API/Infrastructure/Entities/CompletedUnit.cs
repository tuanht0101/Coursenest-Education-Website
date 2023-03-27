using Microsoft.EntityFrameworkCore;

namespace UserData.API.Infrastructure.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

[PrimaryKey(nameof(UnitId), nameof(EnrollmentId))]
public class CompletedUnit
{
	// Relationship
	public int UnitId { get; set; }
	public int EnrollmentId { get; set; }
	public Enrollment Enrollment { get; set; }
}
