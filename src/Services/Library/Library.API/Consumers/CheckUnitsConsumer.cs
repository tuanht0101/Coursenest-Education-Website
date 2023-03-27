using CommonLibrary.API.MessageBus.Commands;
using CommonLibrary.API.MessageBus.Responses;
using Library.API.Infrastructure.Contexts;
using Library.API.Infrastructure.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Library.API.Consumers;

public class CheckUnitsConsumer : IConsumer<CheckUnits>
{
	private readonly DataContext _context;

	public CheckUnitsConsumer(DataContext context)
	{
		_context = context;
	}

	public async Task Consume(ConsumeContext<CheckUnits> context)
	{
		var queries = context.Message.Queries
			.Distinct()
			.GroupBy(x => x.Id)
			.SelectMany(g => g
				.Where(q => g.Count() == 1 || q.IsExam != null));

		var idQueries = queries
			.Select(q => q.Id)
			.Distinct();

		var existing = await _context.Units
			.Select(x => new CheckUnits.Query()
			{
				Id = x.UnitId,
				IsExam = x.GetType() == typeof(Exam)
			})
			.Where(x => idQueries.Contains(x.Id))
			.ToArrayAsync();

		var missing = queries
			.Where(x => x.IsExam != null)
			.Except(existing);

		if (missing.Any())
		{
			await context.RespondAsync(new NotFound()
			{
				Message = $"Some of queried Units are not exist.",
				Objects = missing.ToArray()
			});
		}
		else
		{
			await context.RespondAsync(new Existed());
		}
	}
}
