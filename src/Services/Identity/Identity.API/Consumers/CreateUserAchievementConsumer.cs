using AutoMapper;
using CommonLibrary.API.MessageBus.Commands;
using CommonLibrary.API.MessageBus.Responses;
using Identity.API.Infrastructure.Contexts;
using Identity.API.Infrastructure.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Consumers;

public class CreateUserAchievementConsumer : IConsumer<CreateUserAchievement>
{
	private readonly IMapper _mapper;
	private readonly DataContext _context;

	public CreateUserAchievementConsumer(IMapper mapper, DataContext context)
	{
		_mapper = mapper;
		_context = context;
	}

	public async Task Consume(ConsumeContext<CreateUserAchievement> context)
	{
		var exists = await _context.Users
			.AnyAsync(x => x.UserId == context.Message.UserId);
		if (!exists)
		{
			await context.RespondAsync(new NotFound()
			{
				Message = $"UserId: {context.Message.UserId} does not exist."
			});
			return;
		}

		var achievement = _mapper.Map<Achievement>(context.Message);
		_context.Achievements.Add(achievement);

		await _context.SaveChangesAsync();

		await context.RespondAsync(new Created()
		{
			Id = achievement.AchievementId
		});
	}
}
