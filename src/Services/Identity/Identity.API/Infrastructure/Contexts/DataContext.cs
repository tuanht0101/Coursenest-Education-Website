using Identity.API.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Infrastructure.Contexts;

public class DataContext : DbContext
{
	public DataContext(DbContextOptions<DataContext> options) : base(options)
	{
	}

	public DbSet<Achievement> Achievements { get; set; }
	public DbSet<Avatar> Avatars { get; set; }
	public DbSet<Experience> Experiences { get; set; }
	public DbSet<InterestedTopic> InterestedTopics { get; set; }
	public DbSet<FollowedTopic> FollowedTopics { get; set; }
	public DbSet<User> Users { get; set; }
}
