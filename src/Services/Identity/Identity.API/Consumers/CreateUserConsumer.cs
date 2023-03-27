using AutoMapper;
using CommonLibrary.API.MessageBus.Commands;
using CommonLibrary.API.MessageBus.Responses;
using Identity.API.Infrastructure.Contexts;
using Identity.API.Infrastructure.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Consumers;

public class CreateUserConsumer : IConsumer<CreateUser>
{
	private readonly IMapper _mapper;
	private readonly DataContext _context;

	public CreateUserConsumer(IMapper mapper, DataContext context)
	{
		_mapper = mapper;
		_context = context;
	}

	public async Task Consume(ConsumeContext<CreateUser> context)
	{
		var exists = await _context.Users
			.AnyAsync(x => x.Email == context.Message.Email);
		if (exists)
		{
			await context.RespondAsync(new Existed()
			{
				Message = "Email existed."
			});
			return;
		}

		var user = _mapper.Map<User>(context.Message);
		_context.Users.Add(user);

		await _context.SaveChangesAsync();

		await context.RespondAsync(new Created()
		{
			Id = user.UserId
		});
	}
}
