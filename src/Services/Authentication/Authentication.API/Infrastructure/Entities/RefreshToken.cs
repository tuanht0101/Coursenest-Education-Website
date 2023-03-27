using System.ComponentModel.DataAnnotations;

namespace Authentication.API.Infrastructure.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class RefreshToken
{
	[Key]
	public string Token { get; set; }
	public DateTime Expiry { get; set; }

	// Relationship
	public int CredentialUserId { get; set; }
	public Credential Credential { get; set; }
}
