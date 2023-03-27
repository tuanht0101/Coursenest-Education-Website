using CommonLibrary.API.MessageBus.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using UserData.API.Infrastructure.Contexts;

namespace UserData.API.Consumers;
public class UserDeletedConsumer : IConsumer<UserDeleted>
{
	private readonly DataContext _context;

	public UserDeletedConsumer(DataContext context)
	{
		_context = context;
	}

	public async Task Consume(ConsumeContext<UserDeleted> context)
	{
		await _context.Enrollments
			.Where(x => x.StudentUserId == context.Message.UserId)
			.ExecuteDeleteAsync();
	}
}
