using CommonLibrary.API.MessageBus.Commands;
using CommonLibrary.API.MessageBus.Responses;
using Library.API.Infrastructure.Contexts;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Library.API.Consumers;

public class CheckTopicsConsumer : IConsumer<CheckTopics>
{
	private readonly DataContext _context;

	public CheckTopicsConsumer(DataContext context)
	{
		_context = context;
	}

	public async Task Consume(ConsumeContext<CheckTopics> context)
	{
		var queries = context.Message.Ids
			.Where(x => x > 0)
			.Distinct();

		var existing = await _context.Topics
			.Select(x => x.TopicId)
			.Where(x => queries.Contains(x))
			.ToArrayAsync();

		var missing = queries
			.Except(existing);

		if (missing.Any())
		{
			await context.RespondAsync(new NotFound()
			{
				Message = $"Some of queried Ids are not exist.",
				Objects = missing.ToArray()
			});
		}
		else
		{
			await context.RespondAsync(new Existed());
		}
	}
}
