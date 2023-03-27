using System.ComponentModel.DataAnnotations;

namespace Identity.API.Infrastructure.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class Avatar
{
	public string MediaType { get; set; }
	public byte[] Data { get; set; }

	// Relationship
	[Key]
	public int UserId { get; set; }
	public User User { get; set; }
}
