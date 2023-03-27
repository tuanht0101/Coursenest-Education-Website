using Authentication.API.Infrastructure.Contexts;
using Authentication.API.Infrastructure.Entities;
using CommonLibrary.API.MessageBus.Commands;
using CommonLibrary.API.MessageBus.Responses;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Authentication.API.Consumers;
public class ExtendRoleConsumer : IConsumer<ExtendRole>
{
	private readonly DataContext _context;

	public ExtendRoleConsumer(DataContext context)
	{
		_context = context;
	}

	public async Task Consume(ConsumeContext<ExtendRole> context)
	{
		var credential = await _context.Credentials
			.Include(x => x.Roles)
			.FirstOrDefaultAsync(x => x.UserId == context.Message.UserId);
		if (credential == null)
		{
			await context.RespondAsync(new NotFound()
			{
				Message = $"UserId: {context.Message.UserId} does not exist."
			});
			return;
		}

		var role = credential.Roles
			.FirstOrDefault(x => x.Type == context.Message.Type);
		if (role == null)
		{
			role = new Role()
			{
				Type = context.Message.Type,
				Expiry = DateTime.Now.AddDays(context.Message.ExtendedDays)
			};
			credential.Roles.Add(role);
		}
		else
		{
			var baseExpiry = role.Expiry < DateTime.Now ? DateTime.Now : role.Expiry;
			role.Expiry = baseExpiry.AddDays(context.Message.ExtendedDays);
			_context.Roles.Update(role);
		}

		await _context.SaveChangesAsync();

		await context.RespondAsync(new Succeeded());
	}
}
