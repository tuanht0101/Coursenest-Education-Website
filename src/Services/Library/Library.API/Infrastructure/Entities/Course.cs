using CommonLibrary.API.Models;

namespace Library.API.Infrastructure.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class Course
{
	public int CourseId { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
	public string About { get; set; }
	public CourseTier Tier { get; set; }
	public bool IsApproved { get; set; }
	public DateTime Created { get; set; }
	public DateTime LastModified { get; set; }
	public float RatingAverage { get; set; }
	public int RatingTotal { get; set; }

	// Relationship
	public int? TopicId { get; set; }
	public Topic? Topic { get; set; }
	public CourseCover? Cover { get; set; }
	public List<Lesson> Lessons { get; set; }
	public List<Rating> Ratings { get; set; }
	public int PublisherUserId { get; set; }
}
