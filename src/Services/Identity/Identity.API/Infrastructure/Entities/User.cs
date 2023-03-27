using Microsoft.EntityFrameworkCore;

namespace Identity.API.Infrastructure.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

[Index(nameof(Email), IsUnique = true)]
public class User
{
	public int UserId { get; set; }
	public string Email { get; set; }
	public string? Phonenumber { get; set; }
	public string FullName { get; set; }
	public string? Title { get; set; }
	public string? AboutMe { get; set; }
	public Gender? Gender { get; set; }
	public DateTime? DateOfBirth { get; set; }
	public string? Location { get; set; }
	public DateTime Created { get; set; }
	public DateTime LastModified { get; set; }

	// Relationship
	public Avatar? Avatar { get; set; }
	public List<Achievement> Achievements { get; set; }
	public List<Experience> Experiences { get; set; }
	public List<InterestedTopic> InterestedTopics { get; set; }
	public List<FollowedTopic> FollowedTopics { get; set; }
}

public enum Gender
{
	Male, Female
}
