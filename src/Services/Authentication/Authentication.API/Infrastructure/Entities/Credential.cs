using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Authentication.API.Infrastructure.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

[Index(nameof(Username), IsUnique = true)]
public class Credential
{
	public string Username { get; set; }
	public string Password { get; set; }

	// Relationship
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public int UserId { get; set; }
	public List<Role> Roles { get; set; }
	public List<RefreshToken> RefreshTokens { get; set; }
}
