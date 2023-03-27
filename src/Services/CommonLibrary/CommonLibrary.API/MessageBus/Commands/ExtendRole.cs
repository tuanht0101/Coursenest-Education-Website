using CommonLibrary.API.Models;

namespace CommonLibrary.API.MessageBus.Commands;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record ExtendRole
{
	public int UserId { get; set; }
	public RoleType Type { get; set; }
	public int ExtendedDays { get; set; }
}
