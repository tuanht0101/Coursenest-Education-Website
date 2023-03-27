using CommonLibrary.API.DTOs;

namespace Library.API.DTOs.Ratings;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record RatingQuery : PaginationQuery
{
	public int? CourseId { get; set; }
	public int? UserId { get; set; }
}
