using CommonLibrary.API.MessageBus.Commands;
using CommonLibrary.API.MessageBus.Responses;
using Identity.API.DTOs;
using MassTransit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using CommonLibrary.Tests;
using Identity.API.Infrastructure.Contexts;
using CommonLibrary.API.Models;
using System.Text;
using Microsoft.Net.Http.Headers;

namespace Identity.Tests;

[TestFixture]
public class UsersControllerTests
{
	private WebApplicationFactory<Program> _factory;
	private HttpClient _client;
	private HttpClient _clientAdmin;

	[OneTimeSetUp]
	public async Task Setup()
	{
		_factory = await new WebApplicationFactoryBuilder()
			.AddEFCoreTestServices<DataContext>()
			.AddMassTransitTestServices(x =>
			{
				x.AddHandler<CheckTopics>(context =>
				{
					return context.RespondAsync(new Existed());
				});

				x.AddHandler<GetCredentials>(context =>
				{
					var response = new CredentialResults()
					{
						Credentials = new CredentialResults.CredentialResult[]
						{
							new()
							{
								UserId = 0,
								Username = "A",
								Roles = new[] { RoleType.Student }
							}
						}
					};
					return context.RespondAsync(response);
				});
			})
			.BuildAsync<Program>();
		await _factory.DatabaseInitializeAsync(Defaults.Database);

		_client = _factory.CreateClient();

		_clientAdmin = _factory.CreateClient();
		_clientAdmin.AddRole(new[] { nameof(RoleType.Admin) });
	}


	public static readonly UserQuery[] GetAllAdminData = new[]
	{
		new UserQuery() { FullName = "Em" },
		new UserQuery() { PageNumber = 1, PageSize = 3 },
		new UserQuery()
	};
	[Test]
	[TestCaseSource(nameof(GetAllAdminData))]
	public async Task GetAllAdmin_ReturnsUserAdminResults(UserQuery query)
	{
		// Arrange
		var queries = new List<KeyValuePair<string, string?>>
		{
			new(nameof(UserQuery.PageNumber), query.PageNumber.ToString()),
			new(nameof(UserQuery.PageSize), query.PageSize.ToString())
		};
		if (!string.IsNullOrWhiteSpace(query.FullName))
		{
			queries.Add(new(nameof(UserQuery.FullName), query.FullName));
		}

		var queryString = QueryString.Create(queries);

		// Act
		var results = await _clientAdmin
			.GetFromJsonAsync<IEnumerable<UserAdminResult>>("/users/admin" + queryString.ToString());

		// Assert
		Assert.That(results, Is.Not.Null);
	}

	[Test]
	public async Task GetAdminCount_ReturnsSuccessStatusCode()
	{
		// Arrange

		// Act
		var response = await _clientAdmin
			.GetAsync("/users/admin/count");

		// Assert
		Assert.That(response.IsSuccessStatusCode);
	}

	[Test]
	[TestCase(7)]
	public async Task Delete_ReturnsOk(int userId)
	{
		// Arrange

		// Act
		var response = await _clientAdmin
			.DeleteAsync($"/users/{userId}");

		// Assert
		Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
	}


	[Test]
	[TestCase(new int[] { 1, 2 })]
	[TestCase(new int[] { 1 })]
	public async Task GetAll_ReturnsUserResults(int[] ids)
	{
		// Arrange
		var queries = ids.Select(x => new KeyValuePair<string, string?>("ids", x.ToString()));
		var queryString = QueryString.Create(queries);

		// Act
		var results = await _client
			.GetFromJsonAsync<IEnumerable<UserResult>>("/users" + queryString.ToString());

		// Assert
		Assert.That(results, Is.Not.Null);
	}

	[Test]
	[TestCase(1, "Hanoi")]
	[TestCase(2, "Ho Chi Minh City")]
	public async Task Get_ReturnsExactLocation(int userId, string location)
	{
		// Arrange

		// Act
		var result = await _client
			.GetFromJsonAsync<UserProfileResult>($"/users/{userId}");

		// Assert
		Assert.That(result, Is.Not.Null);
		Assert.That(result.Location, Is.EqualTo(location));
	}

	[Test]
	[TestCase(1, "Developer")]
	[TestCase(2, "Stdent")]
	public async Task GetInstructor_ReturnsExactTitle(int userId, string title)
	{
		// Arrange

		// Act
		var result = await _client.GetFromJsonAsync<UserInstructorResult>($"/users/{userId}/instructor");

		// Assert
		Assert.That(result, Is.Not.Null);
		Assert.That(result.Title, Is.EqualTo(title));
	}


	public static readonly object[] UpdateData = new[]
	{
		new object[] { 2, new UpdateUser() { FullName = "John Smith", Location = "Berlin" } }
	};
	[Test]
	[TestCaseSource(nameof(UpdateData))]
	public async Task Update_ReturnsNoContent(int userId, UpdateUser body)
	{
		// Arrange
		var client = _factory.CreateClient();
		client.AddNameIdentifier(userId);		

		// Act
		var response = await client.PutAsJsonAsync($"/users/me", body);

		// Assert
		Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
	}


	[Test]
	[TestCase(2, 4)]
	public async Task AddInterestedTopic_ReturnsCreated(int userId, int topicId)
	{
		// Arrange
		var client = _factory.CreateClient();
		client.AddNameIdentifier(userId);

		// Act
		var response = await client.PostAsJsonAsync($"/users/me/interest", topicId);

		// Assert
		Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
	}

	[Test]
	[TestCase(2, 3)]
	public async Task DeleteInterestedTopic_ReturnsNoContent(int userId, int topicId)
	{
		// Arrange
		var client = _factory.CreateClient();
		client.AddNameIdentifier(userId);

		// Act
		var response = await client.DeleteAsync($"/users/me/interest/{topicId}");

		// Assert
		Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
	}


	[Test]
	[TestCase(2)]
	[TestCase(3)]
	public async Task GetAllFollowedTopic_ReturnsOk(int userId)
	{
		// Arrange
		var client = _factory.CreateClient();
		client.AddNameIdentifier(userId);

		// Act
		var result = await client.GetFromJsonAsync<IEnumerable<int>>($"/users/me/follow");

		// Assert
		Assert.That(result, Is.Not.Null);
	}

	[Test]
	[TestCase(3, 1)]
	public async Task AddFollowedTopic_ReturnsCreated(int userId, int topicId)
	{
		// Arrange
		var client = _factory.CreateClient();
		client.AddNameIdentifier(userId);

		// Act
		var response = await client.PostAsJsonAsync($"/users/me/follow", topicId);

		// Assert
		Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
	}

	[Test]
	[TestCase(3, 2)]
	public async Task DeleteFollowedTopic_ReturnsNoContent(int userId, int topicId)
	{
		// Arrange
		var client = _factory.CreateClient();
		client.AddNameIdentifier(userId);

		// Act
		var response = await client.DeleteAsync($"/users/me/follow/{topicId}");

		// Assert
		Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
	}


	[Test]
	public async Task AddExperience_ReturnsCreated()
	{
		// Arrange
		var client = _factory.CreateClient();
		client.AddNameIdentifier(2);

		var body = new CreateExperience()
		{
			Name = "School A",
			Title = "Engineer",
			Started = DateTime.Now.AddYears(-1),
			Ended = DateTime.Now
		};

		// Act
		var response = await client.PostAsJsonAsync($"/users/me/experiences", body);

		// Assert
		Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
	}

	[Test]
	[TestCase(2, 2)]
	public async Task DeleteExperience_ReturnsNoContent(int userId, int experienceId)
	{
		// Arrange
		var client = _factory.CreateClient();
		client.AddNameIdentifier(userId);

		// Act
		var response = await client.DeleteAsync($"/users/me/experiences/{experienceId}");

		// Assert
		Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
	}
}
