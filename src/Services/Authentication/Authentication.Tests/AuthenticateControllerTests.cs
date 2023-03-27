using Authentication.API.DTOs;
using Authentication.API.Infrastructure.Contexts;
using CommonLibrary.API.MessageBus.Commands;
using CommonLibrary.API.MessageBus.Responses;
using CommonLibrary.API.Models;
using CommonLibrary.Tests;
using MassTransit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace Authentication.Tests;

[TestFixture]
public class AuthenticateControllerTests
{
	private WebApplicationFactory<Program> _factory;
	private HttpClient _client;

	[OneTimeSetUp]
	public async Task Setup()
	{
		_factory = await new WebApplicationFactoryBuilder()
			.AddEFCoreTestServices<DataContext>()
			.AddMassTransitTestServices(x =>
			{
				x.AddHandler<CheckUsers>(context =>
				{
					return context.RespondAsync(new Existed());
				});

				x.AddHandler<CheckTopics>(context =>
				{
					return context.RespondAsync(new Existed());
				});

				x.AddHandler<CreateUser>(context =>
				{
					return context.RespondAsync(new Created() { Id = 8 });
				});
			})
			.BuildAsync<Program>();
		await _factory.DatabaseInitializeAsync(Defaults.Database);

		_client = _factory.CreateClient();
	}


	[Test]
	public async Task Register_ReturnsCreated()
	{
		// Arrange
		var register = new Register()
		{
			Username = "usrtest",
			Password = "pwdtest",
			Email = "testuser@test.com",
			Fullname = "Test Smith",
			InterestedTopicIds = new[] { 1 }
		};

		// Act
		var response = await _client.PostAsJsonAsync("/authenticate/register", register);

		// Assert
		Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
	}

	[Test]
	public async Task Login_ReturnsTokensResult()
	{
		// Arrange
		var body = new Login() { Username = "loginTest", Password = "pwd" };

		// Act
		var content = await _client
			.PostParsedAsync<Login, TokensResult>("/authenticate/login", body);

		// Assert
		Assert.That(content, Is.Not.Null);
	}

	[Test]
	public async Task Logout_ReturnsOk()
	{
		// Arrange
		var client = _factory.CreateClient();
		client.AddNameIdentifier(2);

		// Act
		var response = await client.PostAsync("/authenticate/logout", null);

		// Assert
		Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
	}

	[Test]
	public async Task Refresh_ReturnsAccessTokenResult()
	{
		// Arrange
		var body = "refreshTestRT";

		// Act
		var content = await _client
			.PostParsedAsync<string, AccessTokenResult>("/authenticate/refresh", body);

		// Assert
		Assert.That(content, Is.Not.Null);
	}

	[Test]
	public async Task ResetPassword_ReturnsNewPassword()
	{
		// Arrange
		var body = 4;
		var client = _factory.CreateClient();
		client.AddRole(new string[] { RoleType.Admin.ToString() });

		// Act
		var response = await client.PutAsJsonAsync("/authenticate/reset-password", body);

		// Assert
		Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
	}

	[Test]
	public async Task ForgotPassword_ReturnsNewPassword()
	{
		// Arrange
		var body = new ForgotPassword() { Email = "", Username = "forgotTest" };

		// Act
		var response = await _client.PutAsJsonAsync("/authenticate/forgot-password", body);

		// Assert
		Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
	}

	[Test]
	public async Task ChangePassword_ReturnsOk()
	{
		// Arrange
		var body = new ChangePassword() { OldPassword = "pwd", NewPassword = "pwdUpdate" };
		var client = _factory.CreateClient();
		client.AddNameIdentifier(6);

		// Act
		var response = await client.PutAsJsonAsync("/authenticate/change-password", body);

		// Assert
		Assert.That(response.IsSuccessStatusCode, Is.True);
	}
}
