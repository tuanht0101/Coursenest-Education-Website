using Library.API.DTOs.Categories;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;
using TestCommonLibrary;

namespace Library.Tests;

public class CategoriesControllerTests
{
	private WebApplicationFactory<Program> _factory;
	private HttpClient _client;

	[OneTimeSetUp]
	public async Task Setup()
	{
		_factory = await new WebApplicationFactoryBuilder()
			.BuildAsync<Program>();
		await _factory.DatabaseInitializeAsync(Defaults.CategoriesDatabase);

		_client = _factory.CreateClient();
	}


	[Test]
	public async Task GetAllHierarchy_ReturnsOk()
	{
		// Arrange

		// Act
		var response = await _client.GetAsync("/categories/hierarchy");
		response.EnsureSuccessStatusCode();
		var body = await response.Content.ReadAsStringAsync();
		var results = JsonSerializer.Deserialize<IEnumerable<CategoryResult>>(body);

		// Assert
		Assert.That(results, Is.Not.Null);
	}

	[Test]
	public async Task GetAll_ReturnsOk()
	{
		// Arrange

		// Act
		var response = await _client.GetAsync("/categories");
		response.EnsureSuccessStatusCode();
		var body = await response.Content.ReadAsStringAsync();
		var results = JsonSerializer.Deserialize<IEnumerable<IdContentResult>>(body);

		// Assert
		Assert.That(results, Is.Not.Null);
	}

	[Test]
	[TestCase(1)]
	[TestCase(2)]
	public async Task Get_ReturnsOk(int categoryId)
	{
		// Arrange

		// Act
		var response = await _client.GetAsync($"/categories/{categoryId}");
		response.EnsureSuccessStatusCode();
		var body = await response.Content.ReadAsStringAsync();
		var results = JsonSerializer.Deserialize<IdContentResult>(body);

		// Assert
		Assert.That(results, Is.Not.Null);
	}


	[Test]
	[TestCase("Architecture")]
	[TestCase("Medical")]
	public async Task Create_ReturnsOk(string content)
	{
		// Arrange
		var jsonContent = JsonContent.Create(content);

		// Act
		var response = await _client.PostAsync($"/categories", jsonContent);
		response.EnsureSuccessStatusCode();
		var body = await response.Content.ReadAsStringAsync();
		var results = JsonSerializer.Deserialize<IdContentResult>(body);

		// Assert
		Assert.That(results, Is.Not.Null);
	}

	[Test]
	[TestCase(1, "Development Updated")]
	[TestCase(2, "Business Updated")]
	public async Task Update_ReturnsOk(int categoryId, string content)
	{
		// Arrange
		var jsonContent = JsonContent.Create(content);

		// Act
		var response = await _client.PutAsync($"/categories/{categoryId}", jsonContent);
		response.EnsureSuccessStatusCode();
		var body = await response.Content.ReadAsStringAsync();
		var results = JsonSerializer.Deserialize<IdContentResult>(body);

		// Assert
		Assert.That(results, Is.Not.Null);
	}

	[Test]
	[TestCase(3)]
	[TestCase(4)]
	public async Task Delete_ReturnsOk(int categoryId)
	{
		// Arrange

		// Act
		var response = await _client.DeleteAsync($"/categories/{categoryId}");

		// Assert
		Assert.That(response.IsSuccessStatusCode, Is.True);
	}



	[Test]
	[TestCase(1)]
	[TestCase(2)]
	public async Task GetAllSubcategory_ReturnsOk(int categoryId)
	{
		// Arrange
		var queries = new List<KeyValuePair<string, string?>>()
		{
			new KeyValuePair<string, string?>(nameof(categoryId), categoryId.ToString())
		};
		var queryString = QueryString.Create(queries);

		// Act
		var response = await _client.GetAsync("/subcategories" + queryString);
		response.EnsureSuccessStatusCode();
		var body = await response.Content.ReadAsStringAsync();
		var results = JsonSerializer.Deserialize<IEnumerable<IdContentResult>>(body);

		// Assert
		Assert.That(results, Is.Not.Null);
	}

	[Test]
	[TestCase(1)]
	[TestCase(3)]
	public async Task GetSubcategory_ReturnsOk(int categoryId)
	{
		// Arrange

		// Act
		var response = await _client.GetAsync($"/subcategories/{categoryId}");
		response.EnsureSuccessStatusCode();
		var body = await response.Content.ReadAsStringAsync();
		var results = JsonSerializer.Deserialize<IdContentResult>(body);

		// Assert
		Assert.That(results, Is.Not.Null);
	}

	public static readonly CreateContentParentId[] CreateSubcategoryData = new[]
	{
		new CreateContentParentId() { ParentId = 1, Content = "New Category 1" },
		new CreateContentParentId() { ParentId = 2, Content = "New Category 2" }
	};
	[Test]
	[TestCaseSource(nameof(CreateSubcategoryData))]
	public async Task CreateSubcategory_ReturnsOk(CreateContentParentId dto)
	{
		// Arrange
		var jsonContent = JsonContent.Create(dto);

		// Act
		var response = await _client.PostAsync($"/subcategories", jsonContent);
		response.EnsureSuccessStatusCode();
		var body = await response.Content.ReadAsStringAsync();
		var results = JsonSerializer.Deserialize<IdContentResult>(body);

		// Assert
		Assert.That(results, Is.Not.Null);
	}

	[Test]
	[TestCase(1, "Web Development Updated")]
	[TestCase(3, "Entrepreneurship Updated")]
	public async Task UpdateSubcategory_ReturnsOk(int subcategoryId, string content)
	{
		// Arrange
		var jsonContent = JsonContent.Create(content);

		// Act
		var response = await _client.PutAsync($"/subcategories/{subcategoryId}", jsonContent);
		response.EnsureSuccessStatusCode();
		var body = await response.Content.ReadAsStringAsync();
		var results = JsonSerializer.Deserialize<IdContentResult>(body);

		// Assert
		Assert.That(results, Is.Not.Null);
	}

	[Test]
	[TestCase(5)]
	[TestCase(6)]
	public async Task DeleteSubcategory_ReturnsOk(int categoryId)
	{
		// Arrange

		// Act
		var response = await _client.DeleteAsync($"/subcategories/{categoryId}");

		// Assert
		Assert.That(response.IsSuccessStatusCode, Is.True);
	}



	[Test]
	[TestCase(1)]
	[TestCase(2)]
	public async Task GetAllTopic_ReturnsOk(int subcategoryId)
	{
		// Arrange
		var queries = new List<KeyValuePair<string, string?>>()
		{
			new KeyValuePair<string, string?>(nameof(subcategoryId), subcategoryId.ToString())
		};
		var queryString = QueryString.Create(queries);

		// Act
		var response = await _client.GetAsync("/topics" + queryString);
		response.EnsureSuccessStatusCode();
		var body = await response.Content.ReadAsStringAsync();
		var results = JsonSerializer.Deserialize<IEnumerable<IdContentResult>>(body);

		// Assert
		Assert.That(results, Is.Not.Null);
	}

	[Test]
	[TestCase(1)]
	[TestCase(3)]
	public async Task GetTopic_ReturnsOk(int topicId)
	{
		// Arrange

		// Act
		var response = await _client.GetAsync($"/subcategories/{topicId}");
		response.EnsureSuccessStatusCode();
		var body = await response.Content.ReadAsStringAsync();
		var results = JsonSerializer.Deserialize<TopicDetailedResult>(body);

		// Assert
		Assert.That(results, Is.Not.Null);
	}

	public static readonly CreateContentParentId[] CreateTopicData = new[]
	{
		new CreateContentParentId() { ParentId = 1, Content = "New Topic 1" },
		new CreateContentParentId() { ParentId = 2, Content = "New Topic 2" }
	};
	[Test]
	[TestCaseSource(nameof(CreateTopicData))]
	public async Task CreateTopic_ReturnsOk(CreateContentParentId dto)
	{
		// Arrange
		var jsonContent = JsonContent.Create(dto);

		// Act
		var response = await _client.PostAsync($"/topics", jsonContent);
		response.EnsureSuccessStatusCode();
		var body = await response.Content.ReadAsStringAsync();
		var results = JsonSerializer.Deserialize<IdContentResult>(body);

		// Assert
		Assert.That(results, Is.Not.Null);
	}

	[Test]
	[TestCase(1, "JavaScript Updated")]
	[TestCase(3, "Python Updated")]
	public async Task UpdateTopic_ReturnsOk(int topicId, string content)
	{
		// Arrange
		var jsonContent = JsonContent.Create(content);

		// Act
		var response = await _client.PutAsync($"/topics/{topicId}", jsonContent);
		response.EnsureSuccessStatusCode();
		var body = await response.Content.ReadAsStringAsync();
		var results = JsonSerializer.Deserialize<IdContentResult>(body);

		// Assert
		Assert.That(results, Is.Not.Null);
	}

	[Test]
	[TestCase(9)]
	[TestCase(10)]
	public async Task DeleteTopic_ReturnsOk(int topicId)
	{
		// Arrange

		// Act
		var response = await _client.DeleteAsync($"/topics/{topicId}");
		response.EnsureSuccessStatusCode();

		// Assert
		Assert.That(response.IsSuccessStatusCode, Is.True);
	}
}
