using CommonLibrary.API.MessageBus.Commands;
using CommonLibrary.API.MessageBus.Responses;
using Library.API.Consumers;
using Library.API.Infrastructure.Contexts;
using MassTransit;
using MassTransit.Testing;
using Microsoft.AspNetCore.Mvc.Testing;
using TestCommonLibrary;

namespace Library.Tests;

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
			.AddMassTransitTestHarness(x =>
			{
				x.AddConsumer<CheckTopicsConsumer>();
			})
			.BuildAsync<Program>();
		await _factory.DatabaseInitializeAsync(Defaults.CategoriesDatabase);

		_harness = _factory.Services.GetTestHarness();
	}


	[Test]
	public async Task CheckTopicsConsumer_ReturnsExisted()
	{
		// Arrange
		var client = _harness.GetRequestClient<CheckTopicIds>();
		var request = new CheckTopicIds()
		{
			TopicIds = new[] { 1, 2, 3, 5 }
		};

		using var scope = _factory.Services.CreateScope();
		var context = scope.ServiceProvider.GetRequiredService<DataContext>();

		// Act
		var response = await client.GetResponse<Existed, NotFound>(request);
		if (response.Is(out Response<NotFound>? notFountResponse))
		{
			throw new Exception(notFountResponse!.Message.Message);
		}

		// Assert
		Assert.That(response.Is(out Response<Existed>? _), Is.True);
	}
}
