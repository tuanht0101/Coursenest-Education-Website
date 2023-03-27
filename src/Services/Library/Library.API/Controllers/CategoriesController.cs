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
	public class CategoriesController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly DataContext _context;

		public CategoriesController(IMapper mapper, DataContext context)
		{
			_mapper = mapper;
			_context = context;
		}


		// GET: /categories/hierarchy
		[HttpGet("hierarchy")]
		public async Task<ActionResult<List<CategoryResult>>> GetAllHierarchy()
		{
			var results = await _context.Categories
				.ProjectTo<CategoryResult>(_mapper.ConfigurationProvider)
				.ToListAsync();

			return results;
		}

		// GET: /categories
		[HttpGet]
		public async Task<ActionResult<List<IdContentResult>>> GetAll()
		{
			var results = await _context.Categories
				.ProjectTo<IdContentResult>(_mapper.ConfigurationProvider)
				.ToListAsync();

			return results;
		}

		// GET: /categories/5
		[HttpGet("{categoryId}")]
		public async Task<ActionResult<IdContentResult>> Get(
			int categoryId)
		{
			var result = await _context.Categories
				.ProjectTo<IdContentResult>(_mapper.ConfigurationProvider)
				.FirstOrDefaultAsync(x => x.Id == categoryId);
			if (result == null)
				return NotFound("CategoryId does not exist.");

			return result;
		}


		// POST: /categories
		[HttpPost]
		[Authorize(Roles = nameof(RoleType.Admin))]
		public async Task<ActionResult> Create(
			[FromBody] string content)
		{
			var category = new Category() { Content = content };

			_context.Categories.Add(category);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(Get), new { category.CategoryId }, null);
		}


		// PUT: /categories/5
		[HttpPut("{categoryId}")]
		[Authorize(Roles = nameof(RoleType.Admin))]
		public async Task<ActionResult> Update(
			int categoryId,
			[FromBody] string content)
		{
			var category = await _context.Categories.FindAsync(categoryId);
			if (category == null)
				return NotFound("CategoryId does not exist.");

			category.Content = content;
			await _context.SaveChangesAsync();

			return NoContent();
		}


		// DELETE: /categories/5
		[HttpDelete("{categoryId}")]
		[Authorize(Roles = nameof(RoleType.Admin))]
		public async Task<ActionResult> Delete(
			int categoryId)
		{
			var result = await _context.Categories
				.Where(x => x.CategoryId == categoryId)
				.ExecuteDeleteAsync();
			if (result == 0)
				return NotFound("CategoryId does not exist.");

			return NoContent();
		}
	}
}
