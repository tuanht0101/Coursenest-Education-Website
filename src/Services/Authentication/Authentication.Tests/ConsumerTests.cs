using CommonLibrary.API.MessageBus.Commands;
using CommonLibrary.API.MessageBus.Events;
using CommonLibrary.API.MessageBus.Responses;
using Authentication.API.Consumers;
using Authentication.API.Infrastructure.Contexts;
using MassTransit;
using MassTransit.Testing;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using CommonLibrary.Tests;
using CommonLibrary.API.Models;

namespace Authentication.Tests;

[TestFixture]
public class ConsumerTests
{
	private WebApplicationFactory<Program> _factory;
	private ITestHarness _harness;

	[OneTimeSetUp]
	public async Task Setup()
	{
		_factory = await new WebApplicationFactoryBuilder()
			.AddEFCoreTestServices<DataContext>()
			.AddMassTransitTestServices(x =>
			{
				x.AddConsumer<ExtendRoleConsumer>();
				x.AddConsumer<GetCredentialsConsumer>();
				x.AddConsumer<UserDeletedConsumer>();
			})
			.BuildAsync<Program>();
		await _factory.DatabaseInitializeAsync(Defaults.Database);

		_harness = _factory.Services.GetTestHarness();
	}


	[Test]
	public async Task ExtendRoleConsumer_ReturnsSucceeded()
	{
		// Arrange
		var client = _harness.GetRequestClient<ExtendRole>();
		var request = new ExtendRole()
		{
			UserId = 1,
			Type = RoleType.Student,
			ExtendedDays = 30
		};

		// Act
		var response = await client.GetResponse<Succeeded, NotFound>(request);
		if (response.Is(out Response<NotFound>? notFoundResponse))
		{
			throw new Exception(notFoundResponse!.Message.Message);
		}

		// Assert
		Assert.That(response.Is(out Response<Succeeded>? _), Is.True);
	}

	[Test]
	public async Task GetCredentialsConsumer_ReturnsExactCount()
	{
		// Arrange
		var client = _harness.GetRequestClient<GetCredentials>();
		var request = new GetCredentials()
		{
			Ids = new[] { 1, 2, 4 }
		};

		// Act
		var response = await client.GetResponse<CredentialResults>(request);

		// Assert
		Assert.That(response.Message.Credentials.Count, Is.EqualTo(request.Ids.Count()));
	}

	[Test]
	public async Task UserDeletedConsumer_ReturnsNothing()
	{
		// Arrange
		using var scope = _factory.Services.CreateScope();
		var endpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
		var request = new UserDeleted()
		{
			UserId = 2
		};

		// Act
		await endpoint.Publish(request);

		// Assert
		Assert.Pass();
	}
}
