using Authentication.API.Infrastructure.Contexts;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CommonLibrary.API.MessageBus.Commands;
using CommonLibrary.API.MessageBus.Responses;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Authentication.API.Consumers;
public class GetCredentialsConsumer : IConsumer<GetCredentials>
{
	private readonly IMapper _mapper;
	private readonly DataContext _context;

	public GetCredentialsConsumer(IMapper mapper, DataContext context)
	{
		_mapper = mapper;
		_context = context;
	}

	public async Task Consume(ConsumeContext<GetCredentials> context)
	{
		var results = await _context.Credentials
			.Where(x => context.Message.Ids.Contains(x.UserId))
			.ProjectTo<CredentialResults.CredentialResult>(_mapper.ConfigurationProvider)
			.ToArrayAsync();

		var response = new CredentialResults()
		{
			Credentials = results
		};
		await context.RespondAsync(response);
	}
}
