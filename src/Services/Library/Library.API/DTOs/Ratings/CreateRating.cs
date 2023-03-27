using System.ComponentModel.DataAnnotations;

namespace Library.API.DTOs.Ratings;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record CreateRating
{
	public int CourseId { get; set; }

	[Range(1, 5)]
	public int Stars { get; set; }

	public string Content { get; set; }
}
