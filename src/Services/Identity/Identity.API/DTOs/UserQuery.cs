using CommonLibrary.API.DTOs;

namespace Identity.API.DTOs;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record UserQuery : PaginationQuery
{
	public string? FullName { get; set; }
}
