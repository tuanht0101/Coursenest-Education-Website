namespace CommonLibrary.API.MessageBus.Responses;

public record NotFound
{
	public string? Message { get; set; }
	public object? Objects { get; set; }
}
