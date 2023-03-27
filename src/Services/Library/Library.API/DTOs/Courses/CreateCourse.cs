using CommonLibrary.API.Models;

namespace Library.API.DTOs.Courses;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record CreateCourse
{
	public string Title { get; set; }
	public string Description { get; set; }
	public string About { get; set; }
	public CourseTier Tier { get; set; }
	public int? TopicId { get; set; }
}
