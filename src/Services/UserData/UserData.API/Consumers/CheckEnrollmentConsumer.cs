using CommonLibrary.API.MessageBus.Commands;
using CommonLibrary.API.MessageBus.Events;
using CommonLibrary.API.MessageBus.Responses;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using UserData.API.Infrastructure.Contexts;
using static MassTransit.ValidationResultExtensions;

namespace UserData.API.Consumers;
public class CheckEnrollmentConsumer : IConsumer<CheckEnrollment>
{
	private readonly DataContext _context;

	public CheckEnrollmentConsumer(DataContext context)
	{
		_context = context;
	}

	public async Task Consume(ConsumeContext<CheckEnrollment> context)
	{
		var query = context.Message;
		var exists = await _context.Enrollments
			.AnyAsync(x => x.CourseId == query.CourseId && x.StudentUserId == query.StudentUserId);

		if (!exists)
		{
			await context.RespondAsync(new NotFound()
			{
				Message = $"CourseId and StudentUserId does not exist together.",
				Objects = query
			});
		}
		else
		{
			await context.RespondAsync(new Existed());
		}
	}
}
