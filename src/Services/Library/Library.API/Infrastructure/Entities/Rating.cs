using Microsoft.EntityFrameworkCore;

namespace Library.API.Infrastructure.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

[PrimaryKey(nameof(CourseId), nameof(UserId))]
public class Rating
{
	public int Stars { get; set; }
	public string Content { get; set; }
	public DateTime Created { get; set; }

	// Relationship
	public int CourseId { get; set; }
	public Course Course { get; set; }
	public int UserId { get; set; }
}
