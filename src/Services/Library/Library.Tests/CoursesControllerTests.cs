using Library.API.DTOs.Courses;
using Library.API.Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;
using TestCommonLibrary;

namespace Library.Tests;

public class CoursesControllerTests
{
	private WebApplicationFactory<Program> _factory;
	private HttpClient _client;

	[OneTimeSetUp]
	public async Task Setup()
	{
		_factory = await new WebApplicationFactoryBuilder()
			.BuildAsync<Program>();
		await _factory.DatabaseInitializeAsync(Defaults.Database);

		_client = _factory.CreateClient();
	}



	public static readonly CourseExtendedQuery[] GetAllData = new[]
	{
		new CourseExtendedQuery()
		{
			TopicId = 1,
		},
		new CourseExtendedQuery()
		{
			PublisherUserId = 1,
		},
		new CourseExtendedQuery()
		{
			Title = "Test",
		},
		new CourseExtendedQuery()
		{
			Page = 1,
			PageSize = 3,
		}
	};
	[Test]
	[TestCaseSource(nameof(GetAllData))]
	public async Task GetAll_ReturnsOk(CourseExtendedQuery query)
	{
		// Arrange
		var queries = new List<KeyValuePair<string, string?>>();
		if (query.TopicId != null)
		{
			queries.Add(new KeyValuePair<string, string?>(nameof(CourseExtendedQuery.TopicId), query.TopicId.ToString()));
		}
		if (query.PublisherUserId != null)
		{
			queries.Add(new KeyValuePair<string, string?>(nameof(CourseExtendedQuery.PublisherUserId), query.PublisherUserId.ToString()));
		}
		if (!string.IsNullOrWhiteSpace(query.Title))
		{
			queries.Add(new KeyValuePair<string, string?>(nameof(CourseExtendedQuery.Title), query.Title.ToString()));
		}
		if (query.Page >= 0)
		{
			queries.Add(new KeyValuePair<string, string?>(nameof(CourseExtendedQuery.Page), query.Page.ToString()));
		}
		if (query.PageSize >= 1)
		{
			queries.Add(new KeyValuePair<string, string?>(nameof(CourseExtendedQuery.PageSize), query.PageSize.ToString()));
		}
		var queryString = QueryString.Create(queries);

		// Act
		var response = await _client.GetAsync("/courses" + queryString);
		response.EnsureSuccessStatusCode();
		var body = await response.Content.ReadAsStringAsync();
		var results = JsonSerializer.Deserialize<IEnumerable<CourseResult>>(body);

		// Assert
		Assert.That(results, Is.Not.Null);
	}


	public static readonly object[] GetAllByPublisherData = new[]
	{
		new object[]
		{
			1,
			new CourseQuery()
		},
		new object[]
		{
			1,
			new CourseQuery()
			{
				Title = "Test",
			}
		},
		new object[]
		{
			1,
			new CourseQuery()
			{
				Page = 1,
				PageSize = 3,
			}
		}
	};
	[Test]
	[TestCaseSource(nameof(GetAllByPublisherData))]
	public async Task GetAllByPublisher_ReturnsOk(int publisherUserId, CourseQuery query)
	{
		// Arrange
		var queries = new List<KeyValuePair<string, string?>>();
		if (!string.IsNullOrWhiteSpace(query.Title))
		{
			queries.Add(new KeyValuePair<string, string?>(nameof(CourseExtendedQuery.Title), query.Title.ToString()));
		}
		if (query.Page >= 0)
		{
			queries.Add(new KeyValuePair<string, string?>(nameof(CourseExtendedQuery.Page), query.Page.ToString()));
		}
		if (query.PageSize >= 1)
		{
			queries.Add(new KeyValuePair<string, string?>(nameof(CourseExtendedQuery.PageSize), query.PageSize.ToString()));
		}
		var queryString = QueryString.Create(queries);

		var client = _factory.CreateClient();
		client.DefaultRequestHeaders.Add("PublisherUserId", publisherUserId.ToString());

		// Act
		var response = await client.GetAsync("/courses/publisher" + queryString);
		response.EnsureSuccessStatusCode();
		var body = await response.Content.ReadAsStringAsync();
		var results = JsonSerializer.Deserialize<IEnumerable<CourseResult>>(body);

		// Assert
		Assert.That(results, Is.Not.Null);
	}


	public static readonly CourseQuery[] GetAllUnapprovedData = new[]
	{
		new CourseQuery(),
		new CourseQuery() { Title = "Test" },
		new CourseQuery() { Page = 1, PageSize = 3 }
	};
	[Test]
	[TestCaseSource(nameof(GetAllUnapprovedData))]
	public async Task GetAllUnapproved_ReturnsOk(CourseQuery query)
	{
		// Arrange
		var queries = new List<KeyValuePair<string, string?>>();
		if (!string.IsNullOrWhiteSpace(query.Title))
		{
			queries.Add(new KeyValuePair<string, string?>(nameof(CourseExtendedQuery.Title), query.Title.ToString()));
		}
		if (query.Page >= 0)
		{
			queries.Add(new KeyValuePair<string, string?>(nameof(CourseExtendedQuery.Page), query.Page.ToString()));
		}
		if (query.PageSize >= 1)
		{
			queries.Add(new KeyValuePair<string, string?>(nameof(CourseExtendedQuery.PageSize), query.PageSize.ToString()));
		}
		var queryString = QueryString.Create(queries);

		// Act
		var response = await _client.GetAsync("/courses/unapproved" + queryString);
		response.EnsureSuccessStatusCode();
		var body = await response.Content.ReadAsStringAsync();
		var results = JsonSerializer.Deserialize<IEnumerable<CourseResult>>(body);

		// Assert
		Assert.That(results, Is.Not.Null);
	}


	[Test]
	[TestCase(null, 1)]
	[TestCase(1, 2)]
	public async Task Get_ReturnsOk(int? publisherUserId, int courseId)
	{
		// Arrange
		var client = _factory.CreateClient();
		client.DefaultRequestHeaders.Add("PublisherUserId", publisherUserId.ToString());

		// Act
		var response = await client.GetAsync($"/courses/{courseId}");
		response.EnsureSuccessStatusCode();
		var body = await response.Content.ReadAsStringAsync();
		var results = JsonSerializer.Deserialize<CourseDetailedResult>(body);

		// Assert
		Assert.That(results, Is.Not.Null);
	}


	[Test]
	[TestCase(1)]
	[TestCase(2)]
	public async Task GetAdmin_ReturnsOk(int courseId)
	{
		// Arrange

		// Act
		var response = await _client.GetAsync($"/courses/{courseId}");
		response.EnsureSuccessStatusCode();
		var body = await response.Content.ReadAsStringAsync();
		var results = JsonSerializer.Deserialize<CourseDetailedResult>(body);

		// Assert
		Assert.That(results, Is.Not.Null);
	}


	public static readonly object[] CreateData = new[]
	{
		new object[]
		{
			1,
			new CreateCourse()
			{
				Title = "Test",
				Description = "Test",
				About = "Test",
				Tier = CourseTier.Free,
				TopicId = 1
			}
		},
		new object[]
		{
			1,
			new CreateCourse()
			{
				Title = "Test",
				Description = "Test",
				About = "Test",
				Tier = CourseTier.Premium
			}
		}
	};
	[Test]
	[TestCaseSource(nameof(CreateData))]
	public async Task Create_ReturnsOk(int publisherUserId, CreateCourse dto)
	{
		// Arrange
		var client = _factory.CreateClient();
		client.DefaultRequestHeaders.Add("PublisherUserId", publisherUserId.ToString());

		var jsonContent = JsonContent.Create(dto);

		// Act
		var response = await client.PostAsync($"/courses", jsonContent);
		response.EnsureSuccessStatusCode();
		var body = await response.Content.ReadAsStringAsync();
		var results = JsonSerializer.Deserialize<CourseDetailedResult>(body);

		// Assert
		Assert.That(results, Is.Not.Null);
	}


	public static readonly object[] UpdateData = new[]
	{
		new object[]
		{
			1,
			1,
			new UpdateCourse()
			{
				Title = "Test",
				Description = "Test"
			}
		},
		new object[]
		{
			1,
			2,
			new UpdateCourse()
			{
				Tier = CourseTier.Premium,
				TopicId = 1
			}
		}
	};
	[Test]
	[TestCaseSource(nameof(UpdateData))]
	public async Task Update_ReturnsOk(int publisherUserId, int courseId, UpdateCourse dto)
	{
		// Arrange
		var client = _factory.CreateClient();
		client.DefaultRequestHeaders.Add("PublisherUserId", publisherUserId.ToString());

		var jsonContent = JsonContent.Create(dto);

		// Act
		var response = await client.PutAsync($"/courses/{courseId}", jsonContent);
		response.EnsureSuccessStatusCode();
		var body = await response.Content.ReadAsStringAsync();
		var results = JsonSerializer.Deserialize<CourseDetailedResult>(body);

		// Assert
		Assert.That(results, Is.Not.Null);
	}


	[Test]
	[TestCase(1, 1)]
	[TestCase(1, 2)]
	public async Task Delete_ReturnsOk(int publisherUserId, int courseId)
	{
		// Arrange
		var client = _factory.CreateClient();
		client.DefaultRequestHeaders.Add("PublisherUserId", publisherUserId.ToString());

		// Act
		var response = await client.DeleteAsync($"/courses/{courseId}");

		// Assert
		Assert.That(response.IsSuccessStatusCode, Is.True);
	}


	//[Test]
	//[TestCase(1, 1)]
	//[TestCase(1, 2)]
	//public async Task Delete_ReturnsOk(int publisherUserId, int courseId)
	//{
	//	// Arrange
	//	var client = _factory.CreateClient();
	//	client.DefaultRequestHeaders.Add("PublisherUserId", publisherUserId.ToString());

	//	// Act
	//	var response = await client.DeleteAsync($"/categories/{courseId}");

	//	// Assert
	//	Assert.That(response.IsSuccessStatusCode, Is.True);
	//}
}
