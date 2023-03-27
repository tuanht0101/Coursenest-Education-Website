using AutoMapper;
using AutoMapper.QueryableExtensions;
using CommonLibrary.API.Models;
using Library.API.DTOs.Categories;
using Library.API.Infrastructure.Contexts;
using Library.API.Infrastructure.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TopicsController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly DataContext _context;

		public TopicsController(IMapper mapper, DataContext context)
		{
			_mapper = mapper;
			_context = context;
		}


		// GET: /topics
		[HttpGet]
		public async Task<ActionResult<List<TopicDetailedResult>>> GetAll(
			[FromQuery] TopicQuery query)
		{
			var results = await _context.Topics
				.Where(x =>
					(query.SubcategoryId == null || x.SubcategoryId == query.SubcategoryId) &&
					(string.IsNullOrWhiteSpace(query.Content) || x.Content.Contains(query.Content)))
				.ProjectTo<TopicDetailedResult>(_mapper.ConfigurationProvider)
				.ToListAsync();

			return results;
		}

		// GET: /topics/5
		[HttpGet("{topicId}")]
		public async Task<ActionResult<TopicDetailedResult>> Get(
			int topicId)
		{
			var result = await _context.Topics
				.ProjectTo<TopicDetailedResult>(_mapper.ConfigurationProvider)
				.FirstOrDefaultAsync(x => x.TopicId == topicId);
			if (result == null)
				return NotFound("TopicId does not exist.");

			return result;
		}


		// POST: /topics
		[HttpPost]
		[Authorize(Roles = nameof(RoleType.Admin))]
		public async Task<ActionResult> Create(
			[FromBody] CreateContentParentId body)
		{
			var topic = new Topic()
			{
				Content = body.Content,
				SubcategoryId = body.ParentId
			};

			_context.Topics.Add(topic);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(Get), new { TopicId = topic.SubcategoryId }, null);
		}


		// PUT: /topics/5
		[HttpPut("{topicId}")]
		[Authorize(Roles = nameof(RoleType.Admin))]
		public async Task<ActionResult> Update(
			int topicId,
			[FromBody] string content)
		{
			var topic = await _context.Topics.FindAsync(topicId);
			if (topic == null)
				return NotFound("TopicId does not exist.");

			topic.Content = content;
			await _context.SaveChangesAsync();

			return NoContent();
		}


		// DELETE: /topics/5
		[HttpDelete("{topicId}")]
		public async Task<ActionResult> Delete(
			int topicId)
		{
			var result = await _context.Topics
				.Where(x => x.TopicId == topicId)
				.ExecuteDeleteAsync();
			if (result == 0)
				return NotFound("TopicId does not exist.");

			return NoContent();
		}
	}
}
