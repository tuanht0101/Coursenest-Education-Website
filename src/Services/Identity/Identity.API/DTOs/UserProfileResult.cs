using Identity.API.Infrastructure.Entities;

namespace Identity.API.DTOs;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record UserProfileResult
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
	public ImageResult? Avatar { get; set; }
	public IEnumerable<AchievementResult> Achievements { get; set; }
	public IEnumerable<ExperienceResult> Experiences { get; set; }
	public IEnumerable<int> InterestedTopics { get; set; }

	public record AchievementResult
	{
		public int AchievementId { get; set; }
		public string Title { get; set; }
		public DateTime Created { get; set; }
	}

	public record ExperienceResult
	{
		public int ExperienceId { get; set; }
		public string Name { get; set; }
		public string Title { get; set; }
		public DateTime Started { get; set; }
		public DateTime? Ended { get; set; }
	}
}
