namespace Library.API.DTOs.Categories;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record CategoryResult
{
	public int CategoryId { get; set; }
	public string Content { get; set; }
	public List<SubcategoryResult> Subcategories { get; set; }

	public record SubcategoryResult
	{
		public int SubcategoryId { get; set; }
		public string Content { get; set; }
		public List<IdContentResult> Topics { get; set; }
	}
}
