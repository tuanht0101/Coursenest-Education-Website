using CommonLibrary.API.MessageBus.Commands;
using CommonLibrary.API.MessageBus.Responses;
using Identity.API.Consumers;
using Identity.API.Infrastructure.Contexts;
using MassTransit;
using MassTransit.Testing;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using CommonLibrary.Tests;

namespace Identity.Tests;

[TestFixture]
[NonParallelizable]
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
				x.AddConsumer<CheckUsersConsumer>();
				x.AddConsumer<CreateUserAchievementConsumer>();
				x.AddConsumer<CreateUserConsumer>();
			})
			.BuildAsync<Program>();
		await _factory.DatabaseInitializeAsync(Defaults.Database);

		_harness = _factory.Services.GetTestHarness();
	}


	[Test]
	public async Task CheckUsersConsumer_ReturnsExisted()
	{
		// Arrange
		var client = _harness.GetRequestClient<CheckUsers>();
		var request = new CheckUsers()
		{
			Queries = new[]
			{
				new CheckUsers.Query()
				{
					Id = 1,
					Email = "one@gmail.com"
				},
				new CheckUsers.Query()
				{
					Id = 2,
					Email = "two@gmail.com"
				},
				new CheckUsers.Query()
				{
					Id = 3
				},
				new CheckUsers.Query()
				{
					Email = "four@gmail.com"
				}
			}
		};

		// Act
		var response = await client.GetResponse<Existed, NotFound>(request);

		// Assert
		Assert.That(response.Is(out Response<Existed>? _), Is.True);
	}

	[Test]
	public async Task CheckUsersConsumer_ReturnsNotFound()
	{
		// Arrange
		var client = _harness.GetRequestClient<CheckUsers>();
		var request = new CheckUsers()
		{
			Queries = new[]
			{
				new CheckUsers.Query()
				{
					Id = 1,
					Email = "two@gmail.com"
				},
				new CheckUsers.Query()
				{
					Id = 2,
					Email = "two@gmail.com"
				},
				new CheckUsers.Query()
				{
					Id = 3
				},
				new CheckUsers.Query()
				{
					Email = "four@gmail.com"
				}
			}
		};

		// Act
		var response = await client.GetResponse<Existed, NotFound>(request);

		// Assert
		Assert.That(response.Is(out Response<NotFound>? _), Is.True);
	}

	[Test]
	public async Task CreateUserAchievementConsumer_ReturnsCreated()
	{
		// Arrange
		var client = _harness.GetRequestClient<CreateUserAchievement>();
		var request = new CreateUserAchievement()
		{
			Title = "Learned CSS.",
			Created = DateTime.Now,
			UserId = 1
		};

		// Act
		var response = await client.GetResponse<Created, NotFound>(request);
		if (response.Is(out Response<NotFound>? notFountResponse))
		{
			throw new Exception(notFountResponse!.Message.Message);
		}

		// Assert
		Assert.That(response.Is(out Response<Created>? _), Is.True);
	}

	[Test]
	public async Task CreateUserConsumer_ReturnsCreated()
	{
		// Arrange
		var client = _harness.GetRequestClient<CreateUser>();
		var request = new CreateUser()
		{
			Email = "testing@test.com",
			FullName = "Tester A",
			InterestedTopicIds = new[] { 1, 2 }
		};

		// Act
		var response = await client.GetResponse<Created, Existed>(request);
		if (response.Is(out Response<Existed>? existedResponse))
		{
			throw new Exception(existedResponse!.Message.Message);
		}

		// Assert
		Assert.That(response.Is(out Response<Created>? _), Is.True);
	}
}
