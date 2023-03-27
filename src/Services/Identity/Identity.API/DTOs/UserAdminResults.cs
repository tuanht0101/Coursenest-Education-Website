using CommonLibrary.API.Models;

namespace Identity.API.DTOs;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record UserAdminResults
{
	public IEnumerable<UserAdminResult> Queried { get; set; }
	public int Count { get; set; }

	public record UserAdminResult
	{
		public int UserId { get; set; }
		public string FullName { get; set; }
		public ImageResult? Avatar { get; set; }
		public string Email { get; set; }
		public string Username { get; set; }
		public RoleResult[] Roles { get; set; }
	}

	public record RoleResult
	{
		public RoleType Type { get; set; }
		public DateTime Expiry { get; set; }
	}
}
