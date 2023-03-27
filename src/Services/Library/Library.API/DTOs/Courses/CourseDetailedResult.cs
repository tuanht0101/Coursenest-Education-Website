using CommonLibrary.API.Models;

namespace Library.API.DTOs.Courses;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record CourseDetailedResult
{
	public int CourseId { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
	public string About { get; set; }
	public CourseTier Tier { get; set; }
	public DateTime LastModified { get; set; }
	public float RatingAverage { get; set; }
	public int RatingTotal { get; set; }
	public int? TopicId { get; set; }
	public int PublisherUserId { get; set; }
}
