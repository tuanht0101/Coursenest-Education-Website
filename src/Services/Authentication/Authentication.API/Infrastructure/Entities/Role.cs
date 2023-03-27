using CommonLibrary.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Authentication.API.Infrastructure.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

[PrimaryKey(nameof(CredentialUserId), nameof(Type))]
public class Role
{
	public RoleType Type { get; set; }
	public DateTime Expiry { get; set; }

	// Relationship
	public int CredentialUserId { get; set; }
	public Credential Credential { get; set; }
}
