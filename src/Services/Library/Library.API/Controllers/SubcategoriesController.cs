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
	public class SubcategoriesController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly DataContext _context;

		public SubcategoriesController(IMapper mapper, DataContext context)
		{
			_mapper = mapper;
			_context = context;
		}


		// GET: /subcategories
		[HttpGet]
		public async Task<ActionResult<List<IdContentResult>>> GetAll(
			[FromQuery] int categoryId)
		{
			var results = await _context.Subcategories
				.Where(x => x.CategoryId == categoryId)
				.ProjectTo<IdContentResult>(_mapper.ConfigurationProvider)
				.ToListAsync();

			return results;
		}

		// GET: /subcategories/5
		[HttpGet("{subcategoryId}")]
		public async Task<ActionResult<IdContentResult>> Get(
			int subcategoryId)
		{
			var result = await _context.Subcategories
				.ProjectTo<IdContentResult>(_mapper.ConfigurationProvider)
				.FirstOrDefaultAsync(x => x.Id == subcategoryId);
			if (result == null)
				return NotFound("SubcategoryId does not exist.");

			return result;
		}


		// POST: /subcategories
		[HttpPost]
		[Authorize(Roles = nameof(RoleType.Admin))]
		public async Task<ActionResult> Create(
			[FromBody] CreateContentParentId body)
		{
			var subcategory = new Subcategory()
			{
				Content = body.Content,
				CategoryId = body.ParentId
			};

			_context.Subcategories.Add(subcategory);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(Get), new { subcategory.SubcategoryId }, null);
		}


		// PUT: /subcategories/5
		[HttpPut("{subcategoryId}")]
		[Authorize(Roles = nameof(RoleType.Admin))]
		public async Task<ActionResult> Update(
			int subcategoryId,
			[FromBody] string content)
		{
			var subcategory = await _context.Subcategories.FindAsync(subcategoryId);
			if (subcategory == null)
				return NotFound("SubcategoryId does not exist.");

			subcategory.Content = content;
			await _context.SaveChangesAsync();

			return NoContent();
		}


		// DELETE: /subcategories/5
		[HttpDelete("{subcategoryId}")]
		public async Task<ActionResult> Delete(
			int subcategoryId)
		{
			var result = await _context.Subcategories
				.Where(x => x.SubcategoryId == subcategoryId)
				.ExecuteDeleteAsync();
			if (result == 0)
				return NotFound("SubcategoryId does not exist.");

			return NoContent();
		}
	}
}
