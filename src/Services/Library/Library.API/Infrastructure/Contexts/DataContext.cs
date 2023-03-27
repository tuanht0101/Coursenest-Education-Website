using Library.API.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.API.Infrastructure.Contexts;

public class DataContext : DbContext
{
	public DataContext(DbContextOptions<DataContext> options) : base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		builder.Entity<Lesson>()
			.Property(x => x.Order)
			.HasColumnType("float")
			.HasComputedColumnSql($"CAST(([{nameof(Lesson.OrderNumerator)}] * 1.0 / [{nameof(Lesson.OrderDenominator)}]) AS float)", stored: true);

		builder.Entity<Unit>()
			.Property(x => x.Order)
			.HasColumnType("float")
			.HasComputedColumnSql($"CAST(([{nameof(Lesson.OrderNumerator)}] * 1.0 / [{nameof(Lesson.OrderDenominator)}]) AS float)", stored: true);
	}


	public DbSet<Category> Categories { get; set; }
	public DbSet<Subcategory> Subcategories { get; set; }
	public DbSet<Topic> Topics { get; set; }
	public DbSet<Course> Courses { get; set; }
	public DbSet<Lesson> Lessons { get; set; }
	public DbSet<Unit> Units { get; set; }
	public DbSet<Material> Materials { get; set; }
	public DbSet<Exam> Exams { get; set; }
	public DbSet<Question> Questions { get; set; }
	public DbSet<Choice> Choices { get; set; }
	public DbSet<CourseCover> CourseCovers { get; set; }
	public DbSet<Rating> Ratings { get; set; }
}
