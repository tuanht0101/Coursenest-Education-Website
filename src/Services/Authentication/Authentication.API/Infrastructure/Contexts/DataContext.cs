using Authentication.API.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Authentication.API.Infrastructure.Contexts;

public class DataContext : DbContext
{
	public DataContext(DbContextOptions<DataContext> options) : base(options)
	{
	}

	public DbSet<Credential> Credentials { get; set; }
	public DbSet<Role> Roles { get; set; }
	public DbSet<RefreshToken> RefreshTokens { get; set; }
}
