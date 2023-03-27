using System.Net.Http.Json;
using System.Security.Claims;

namespace CommonLibrary.Tests;
public static class HttpClientExtensions
{
	public static async Task<TResponse?> PostParsedAsync<TRequest, TResponse>(
		this HttpClient client,
		string? requestUri,
		TRequest content)
		where TResponse : class
	{
		var response = await client.PostAsJsonAsync(requestUri, content);
		response.EnsureSuccessStatusCode();
		var result = await response.Content.ReadFromJsonAsync<TResponse>();

		return result;
	}

	public static async Task<TResponse?> PutParsedAsync<TRequest, TResponse>(
		this HttpClient client,
		string? requestUri,
		TRequest content)
		where TResponse : class
	{
		var response = await client.PutAsJsonAsync(requestUri, content);
		response.EnsureSuccessStatusCode();
		var result = await response.Content.ReadFromJsonAsync<TResponse>();

		return result;
	}

	public static void AddNameIdentifier(this HttpClient client, int userId)
	{
		client.DefaultRequestHeaders.Add(nameof(ClaimTypes.NameIdentifier), userId.ToString());
	}

	public static void AddRole(this HttpClient client, IEnumerable<string?> roles)
	{
		client.DefaultRequestHeaders.Add(nameof(ClaimTypes.Role), roles);
	}
}
