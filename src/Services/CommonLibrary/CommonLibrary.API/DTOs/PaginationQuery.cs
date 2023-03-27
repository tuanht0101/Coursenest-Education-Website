using System.ComponentModel.DataAnnotations;

namespace CommonLibrary.API.DTOs;
public record PaginationQuery
{
	[Range(1, int.MaxValue)]
	public int PageNumber { get; set; } = 1;

	[Range(1, int.MaxValue)]
	public int PageSize { get; set; } = 5;
}
