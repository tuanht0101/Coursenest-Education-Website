namespace Identity.API.Infrastructure.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class Achievement
{
	public int AchievementId { get; set; }
	public string Title { get; set; }
	public DateTime Created { get; set; }

	// Relationship
	public int UserId { get; set; }
	public User User { get; set; }
}
