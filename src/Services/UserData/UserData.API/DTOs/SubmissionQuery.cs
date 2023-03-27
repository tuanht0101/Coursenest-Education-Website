using CommonLibrary.API.DTOs;
using CommonLibrary.API.Models;

namespace UserData.API.DTOs;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record SubmissionQuery : PaginationQuery
{
	public string? Title { get; set; }
	public int[] TopicIds { get; set; }
	public int? StudentUserId { get; set; }
	public SortBy SortBy { get; set; } = SortBy.Rating;
}
