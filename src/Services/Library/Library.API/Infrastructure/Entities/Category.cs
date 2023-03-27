namespace Library.API.Infrastructure.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class Category
{
	public int CategoryId { get; set; }
	public string Content { get; set; }

	// Relationship
	public List<Subcategory> Subcategories { get; set; }
}
