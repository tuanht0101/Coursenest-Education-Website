using MassTransit;
using MassTransit.Testing;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;

namespace CommonLibrary.Tests;
public class WebApplicationFactoryBuilder
{
	private Action<IServiceCollection>? _efcoreAction;
	private Action<IServiceCollection>? _massTransitAction;

	public WebApplicationFactoryBuilder AddEFCoreTestServices<TDbContext>()
		where TDbContext : DbContext
	{
		_efcoreAction = services =>
		{
			services.AddSingleton<DbConnection>(_ =>
			{
				var connection = new SqliteConnection("Filename=:memory:");
				connection.Open();

				return connection;
			});

			services.AddDbContext<DbContext, TDbContext>((container, options) =>
			{
				var connection = container.GetRequiredService<DbConnection>();
				options.UseSqlite(connection);
			});
		};

		return this;
	}

	public WebApplicationFactoryBuilder AddMassTransitTestServices(
		Action<IBusRegistrationConfigurator> bus)
	{
		_massTransitAction = services =>
		{
			services.AddMassTransitTestHarness(bus);
		};

		return this;
	}

	public async Task<WebApplicationFactory<TEntryPoint>> BuildAsync<TEntryPoint>()
		where TEntryPoint : class
	{
		var factory = new WebApplicationFactory<TEntryPoint>()
			.WithWebHostBuilder(config =>
			{
				config.UseEnvironment("Development");

				config.ConfigureTestServices(services =>
				{
					_efcoreAction?.Invoke(services);
					_massTransitAction?.Invoke(services);

					services.AddAuthentication(TestAuthenticationHandler.SchemeName)
						.AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>(
							TestAuthenticationHandler.SchemeName, options => { });
				});
			});

		if (_massTransitAction != null)
		{
			var harness = factory.Services.GetTestHarness();
			await harness.Start();
		}

		return factory;
	}
}
