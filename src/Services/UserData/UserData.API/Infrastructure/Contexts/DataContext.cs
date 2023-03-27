using Microsoft.EntityFrameworkCore;
using UserData.API.Infrastructure.Entities;

namespace UserData.API.Infrastructure.Contexts;

public class DataContext : DbContext
{
	public DataContext(DbContextOptions<DataContext> opts) : base(opts)
	{
	}

	public DbSet<Enrollment> Enrollments { get; set; }
	public DbSet<CompletedUnit> CompletedUnits { get; set; }
	public DbSet<Submission> Submissions { get; set; }
	public DbSet<Question> Questions { get; set; }
	public DbSet<Choice> Choices { get; set; }
	public DbSet<Review> Reviews { get; set; }
	public DbSet<Comment> Comments { get; set; }
}
