using CommonLibrary.API.MessageBus.Commands;
using CommonLibrary.API.MessageBus.Responses;
using Library.API.Infrastructure.Contexts;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Library.API.Consumers;

public class CheckCoursesConsumer : IConsumer<CheckCourses>
{
	private readonly DataContext _context;

	public CheckCoursesConsumer(DataContext context)
	{
		_context = context;
	}

	public async Task Consume(ConsumeContext<CheckCourses> context)
	{
		var queries = context.Message.Queries
			.Distinct()
			.GroupBy(x => x.Id)
			.SelectMany(g => g
				.Where(q => g.Count() == 1 || q.IsApproved != null));

		var idQueries = queries
			.Select(q => q.Id)
			.Distinct();

		var existing = await _context.Courses
			.Select(x => new CheckCourses.Query()
			{
				Id = x.CourseId,
				IsApproved = x.IsApproved
			})
			.Where(x => idQueries.Contains(x.Id))
			.ToArrayAsync();

		var missing = queries
			.Where(x => x.IsApproved != null)
			.Except(existing);

		if (missing.Any())
		{
			await context.RespondAsync(new NotFound()
			{
				Message = $"Some of queried Courses are not exist.",
				Objects = missing.ToArray()
			});
		}
		else
		{
			await context.RespondAsync(new Existed());
		}
	}
}
