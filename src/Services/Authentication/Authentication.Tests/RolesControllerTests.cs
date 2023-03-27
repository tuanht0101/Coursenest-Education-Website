using Authentication.API.DTOs;
using Authentication.API.Infrastructure.Contexts;
using CommonLibrary.API.Models;
using CommonLibrary.Tests;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace Authentication.Tests;

[TestFixture]
public class RolesControllerTests
{
	private WebApplicationFactory<Program> _factory;
	private HttpClient _clientAdmin;

	[OneTimeSetUp]
	public async Task Setup()
	{
		_factory = await new WebApplicationFactoryBuilder()
			.AddEFCoreTestServices<DataContext>()
			.BuildAsync<Program>();
		await _factory.DatabaseInitializeAsync(Defaults.Database);

		_clientAdmin = _factory.CreateClient();
		_clientAdmin.AddRole(new string[] { RoleType.Admin.ToString() });
	}


	[Test]
	public async Task GetAll_ReturnsRoleResults()
	{
		// Arrange
		var userId = 1;

		// Act
		var content = await _clientAdmin
			.GetFromJsonAsync<IEnumerable<RoleResult>>($"/roles/{userId}");

		// Assert
		Assert.That(content, Is.Not.Null);
	}

	[Test]
	public async Task GetAllMe_ReturnsOk()
	{
		// Arrange
		var client = _factory.CreateClient();
		client.AddNameIdentifier(1);

		// Act
		var content = await client
			.GetFromJsonAsync<IEnumerable<RoleResult>>("/roles/me");

		// Assert
		Assert.That(content, Is.Not.Null);
	}

	[Test]
	public async Task Update_ReturnsCreated()
	{
		// Arrange
		int userId = 1;
		var body = new SetRole()
		{
			Type = RoleType.Student,
			Expiry = DateTime.Now.AddHours(1)
		};

		// Act
		var response = await _clientAdmin
			.PutAsJsonAsync($"/roles/{userId}", body);

		// Assert
		Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
	}
}
