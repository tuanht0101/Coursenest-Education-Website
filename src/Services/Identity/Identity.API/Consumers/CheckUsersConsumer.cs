using CommonLibrary.API.MessageBus.Commands;
using CommonLibrary.API.MessageBus.Responses;
using Identity.API.Infrastructure.Contexts;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Consumers;

public class CheckUsersConsumer : IConsumer<CheckUsers>
{
	private readonly DataContext _context;

	public CheckUsersConsumer(DataContext context)
	{
		_context = context;
	}

	public async Task Consume(ConsumeContext<CheckUsers> context)
	{
		var validQueries = context.Message.Queries
			.Where(q =>
				(q.Id == null || q.Id > 0) &&
				(q.Email == null || !string.IsNullOrWhiteSpace(q.Email)) &&
				!(q.Id == null && q.Email == null))
			.Distinct();


		var fullQueries = validQueries
			.Where(q => q.Id != null && q.Email != null);

		var idOnlyQueries = validQueries
			.Where(q =>
				q.Id != null &&
				q.Email == null &&
				!fullQueries.Select(x => x.Id).Contains(q.Id));

		var emailOnlyQueries = validQueries
			.Where(q =>
				q.Id == null &&
				q.Email != null &&
				!fullQueries.Select(x => x.Email).Contains(q.Email));

		var queries = fullQueries
			.Concat(idOnlyQueries)
			.Concat(emailOnlyQueries)
			.ToList();


		var idQueries = queries
			.Where(x => x.Id != null)
			.Select(x => x.Id);

		var emailQueries = queries
			.Where(x => x.Email != null)
			.Select(x => x.Email);

		var users = await _context.Users
			.Select(x => new CheckUsers.Query() { Id = x.UserId, Email = x.Email })
			.Where(x =>
				idQueries.Contains(x.Id) ||
				emailQueries.Contains(x.Email))
			.ToListAsync();

		var missingFull = fullQueries
			.Except(users);

		var missingId = idOnlyQueries
			.ExceptBy(users.Select(x => x.Id), x => x.Id);

		var missingEmail = emailOnlyQueries
			.ExceptBy(users.Select(x => x.Email), x => x.Email);

		var missingQueries = missingFull
			.Concat(missingId)
			.Concat(missingEmail);

		if (missingQueries.Any())
		{
			var response = new NotFound();
			response.Message += $"{missingQueries.Count()} queries do not exist.";
			response.Objects = missingQueries;
			await context.RespondAsync(new NotFound()
			{
				Message = $"{missingQueries.Count()} queries do not exist.",
				Objects = missingQueries
			});
		}
		else
		{
			await context.RespondAsync(new Existed());
		}
	}
}
