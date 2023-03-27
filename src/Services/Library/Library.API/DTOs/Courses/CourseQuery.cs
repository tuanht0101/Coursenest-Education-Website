using CommonLibrary.API.DTOs;
using CommonLibrary.API.Models;

namespace Library.API.DTOs.Courses;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record CourseQuery : PaginationQuery
{
	public string? Title { get; set; }
	public int? TopicId { get; set; }
	public int? PublisherUserId { get; set; }
	public bool IsApproved { get; set; } = true;
	public SortBy SortBy { get; set; } = SortBy.Rating;
}
