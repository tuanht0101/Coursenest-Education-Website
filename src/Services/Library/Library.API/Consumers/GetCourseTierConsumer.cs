using CommonLibrary.API.MessageBus.Commands;
using CommonLibrary.API.MessageBus.Responses;
using Library.API.Infrastructure.Contexts;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Library.API.Consumers;

public class GetCourseTierConsumer : IConsumer<GetCourseTier>
{
	private readonly DataContext _context;

	public GetCourseTierConsumer(DataContext context)
	{
		_context = context;
	}

	public async Task Consume(ConsumeContext<GetCourseTier> context)
	{
		var query = context.Message;

		var result = await _context.Courses
			.Where(x =>
				x.CourseId == query.Id &&
				(query.IsApproved == null || query.IsApproved == x.IsApproved))
			.Select(x => new CourseTierResult() { Tier = x.Tier })
			.SingleOrDefaultAsync();
		if (result == null)
		{
			await context.RespondAsync(new NotFound()
			{
				Message = $"Queried Courses does not exist.",
				Objects = query
			});
		}
		else
		{
			await context.RespondAsync(result);
		}
	}
}
