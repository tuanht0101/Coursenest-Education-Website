using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Authentication.API.Infrastructure.Contexts;

public class DataContextDesignTimeFactory : IDesignTimeDbContextFactory<DataContext>
{
	public DataContext CreateDbContext(string[] args)
	{
		var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
		optionsBuilder.UseSqlServer(args.Length != 0 ? args[0] : null);

		return new DataContext(optionsBuilder.Options);
	}
}
