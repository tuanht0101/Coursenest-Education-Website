using Library.API.DTOs;
using Library.API.DTOs.Lessons;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;
using TestCommonLibrary;

namespace Library.Tests;

public class LessonsControllerTests
{
	private WebApplicationFactory<Program> _factory;

	[OneTimeSetUp]
	public async Task Setup()
	{
		_factory = await new WebApplicationFactoryBuilder()
			.BuildAsync<Program>();
		await _factory.DatabaseInitializeAsync(Defaults.Database);
	}


	[Test]
	[TestCase(null, 1)]
	[TestCase(1, 2)]
	public async Task GetAll_ReturnsOk(int? publisherUserId, int courseId)
	{
		// Arrange
		var queries = new List<KeyValuePair<string, string?>>()
		{
			new(nameof(courseId), courseId.ToString())
		};
		var queryString = QueryString.Create(queries);

		var client = _factory.CreateClient();
		if (publisherUserId > 0)
		{
			client.DefaultRequestHeaders.Add("PublisherUserId", publisherUserId.ToString());
		};

		// Act
		var response = await client.GetAsync("/lessons" + queryString);
		response.EnsureSuccessStatusCode();
		var body = await response.Content.ReadAsStringAsync();
		var results = JsonSerializer.Deserialize<IEnumerable<LessonResult>>(body);

		// Assert
		Assert.That(results, Is.Not.Null);
	}


	[Test]
	[TestCase(null, 1)]
	[TestCase(1, 2)]
	public async Task Get_ReturnsOk(int? publisherUserId, int lessonId)
	{
		// Arrange
		var client = _factory.CreateClient();
		if (publisherUserId > 0)
		{
			client.DefaultRequestHeaders.Add("PublisherUserId", publisherUserId.ToString());
		};

		// Act
		var response = await client.GetAsync($"/lessons/{lessonId}");
		response.EnsureSuccessStatusCode();
		var body = await response.Content.ReadAsStringAsync();
		var results = JsonSerializer.Deserialize<LessonResult>(body);

		// Assert
		Assert.That(results, Is.Not.Null);
	}


	public static readonly object[] CreateData = new[]
	{
		new object[]
		{
			1,
			new CreateLesson()
			{
				Title = "Test",
				Description = "Test",
				CourseId = 1,
			}
		},
		new object[]
		{
			1,
			new CreateLesson()
			{
				Title = "Test",
				Description = "Test",
				CourseId = 2,
			}
		}
	};
	[Test]
	[TestCaseSource(nameof(CreateData))]
	public async Task Create_ReturnsOk(int publisherUserId, CreateLesson dto)
	{
		// Arrange
		var client = _factory.CreateClient();
		client.DefaultRequestHeaders.Add("PublisherUserId", publisherUserId.ToString());

		var jsonContent = JsonContent.Create(dto);

		// Act
		var response = await client.PostAsync($"/lessons", jsonContent);
		response.EnsureSuccessStatusCode();
		var body = await response.Content.ReadAsStringAsync();
		var results = JsonSerializer.Deserialize<LessonResult>(body);

		// Assert
		Assert.That(results, Is.Not.Null);
	}


	public static readonly object[] UpdateData = new[]
	{
		new object[]
		{
			1,
			1,
			new UpdateLesson()
			{
				Title = "Test",
				Description = "Test"
			}
		},
		new object[]
		{
			2,
			1,
			new UpdateLesson()
			{
				Title = "Test",
				Description = "Test"
			}
		}
	};
	[Test]
	[TestCaseSource(nameof(UpdateData))]
	public async Task Update_ReturnsOk(int publisherUserId, int lessonId, UpdateLesson dto)
	{
		// Arrange
		var client = _factory.CreateClient();
		client.DefaultRequestHeaders.Add("PublisherUserId", publisherUserId.ToString());

		var jsonContent = JsonContent.Create(dto);

		// Act
		var response = await client.PutAsync($"/lessons/{lessonId}", jsonContent);
		response.EnsureSuccessStatusCode();
		var body = await response.Content.ReadAsStringAsync();
		var results = JsonSerializer.Deserialize<LessonResult>(body);

		// Assert
		Assert.That(results, Is.Not.Null);
	}


	[Test]
	[TestCase(1, 1)]
	[TestCase(1, 2)]
	public async Task Delete_ReturnsOk(int publisherUserId, int lessonId)
	{
		// Arrange
		var client = _factory.CreateClient();
		client.DefaultRequestHeaders.Add("PublisherUserId", publisherUserId.ToString());

		// Act
		var response = await client.DeleteAsync($"/lessons/{lessonId}");

		// Assert
		Assert.That(response.IsSuccessStatusCode, Is.True);
	}


	public static readonly object[] ChangeOrderData = new[]
	{
		new object[]
		{
			1,
			1,
			new ChangeOrder()
			{
				ToId = 1,
				IsBefore = false
			}
		},
		new object[]
		{
			1,
			2,
			new ChangeOrder()
			{
				ToId = 1,
				IsBefore = true
			}
		}
	};
	[Test]
	[TestCaseSource(nameof(ChangeOrderData))]
	public async Task ChangeOrder_ReturnsOk(int publisherUserId, int lessonId, ChangeOrder dto)
	{
		// Arrange
		var client = _factory.CreateClient();
		client.DefaultRequestHeaders.Add("PublisherUserId", publisherUserId.ToString());

		var jsonContent = JsonContent.Create(dto);

		// Act
		var response = await client.PutAsync($"/lessons/{lessonId}/order", jsonContent);
		response.EnsureSuccessStatusCode();
		var body = await response.Content.ReadAsStringAsync();
		var results = JsonSerializer.Deserialize<LessonResult>(body);

		// Assert
		Assert.That(results, Is.Not.Null);
	}
}
