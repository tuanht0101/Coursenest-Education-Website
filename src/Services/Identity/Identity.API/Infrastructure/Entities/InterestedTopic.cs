using Microsoft.EntityFrameworkCore;

namespace Identity.API.Infrastructure.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

[PrimaryKey(nameof(UserId), nameof(TopicId))]
public class InterestedTopic
{
	// Relationship
	public int UserId { get; set; }
	public User User { get; set; }

	// External
	public int TopicId { get; set; }
}
