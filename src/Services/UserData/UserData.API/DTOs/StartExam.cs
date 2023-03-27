namespace UserData.API.DTOs;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record StartExam
{
	public int ExamUnitId { get; set; }
	public int EnrollmentId { get; set; }
}
