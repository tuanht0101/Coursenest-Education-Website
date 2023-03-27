using Authentication.API.Infrastructure.Contexts;
using CommonLibrary.API.MessageBus.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Authentication.API.Consumers;
public class UserDeletedConsumer : IConsumer<UserDeleted>
{
	private readonly DataContext _context;

	public UserDeletedConsumer(DataContext context)
	{
		_context = context;
	}

	public async Task Consume(ConsumeContext<UserDeleted> context)
	{
		await _context.Credentials
			.Where(x => x.UserId == context.Message.UserId)
			.ExecuteDeleteAsync();
	}
}
