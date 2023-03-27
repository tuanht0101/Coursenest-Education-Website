using CommonLibrary.API.Models;

namespace CommonLibrary.API.MessageBus.Responses;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record CredentialResults
{
	public CredentialResult[] Credentials { get; set; }

	public record CredentialResult
	{
		public int UserId { get; set; }
		public string Username { get; set; }
		public RoleResult[] Roles { get; set; }
	}

	public record RoleResult
	{
		public RoleType Type { get; set; }
		public DateTime Expiry { get; set; }
	}
}
