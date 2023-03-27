using System.ComponentModel.DataAnnotations;

namespace Library.API.Infrastructure.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class CourseCover
{
	public string MediaType { get; set; }
	public byte[] Data { get; set; }

	// Relationship
	[Key]
	public int CourseId { get; set; }
	public Course Course { get; set; }
}
