using AutoMapper;
using AutoMapper.QueryableExtensions;
using CommonLibrary.API.MessageBus.Commands;
using CommonLibrary.API.MessageBus.Responses;
using Library.API.Infrastructure.Contexts;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Library.API.Consumers;

public class GetExamConsumer : IConsumer<GetExam>
{
	private readonly IMapper _mapper;
	private readonly DataContext _context;

	public GetExamConsumer(IMapper mapper, DataContext context)
	{
		_mapper = mapper;
		_context = context;
	}

	public async Task Consume(ConsumeContext<GetExam> context)
	{
		var query = context.Message.UnitId;

		var result = await _context.Exams
			.Where(x => x.UnitId == query && x.Lesson.Course.IsApproved)
			.ProjectTo<ExamResult>(_mapper.ConfigurationProvider)
			.FirstOrDefaultAsync();
		if (result == null)
		{
			await context.RespondAsync(new NotFound()
			{
				Message = $"UnitId does not exist or you're not authorized.",
				Objects = query
			});
		}
		else
		{
			await context.RespondAsync(result);
		}
	}
}
