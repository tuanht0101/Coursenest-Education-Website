namespace Library.API.Infrastructure.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class Topic
{
	public int TopicId { get; set; }
	public string Content { get; set; }

	// Relationship
	public int SubcategoryId { get; set; }
	public Subcategory Subcategory { get; set; }
}
