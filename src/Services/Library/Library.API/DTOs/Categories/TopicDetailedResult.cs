namespace Library.API.DTOs.Categories;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record TopicDetailedResult
{
	public int TopicId { get; set; }
	public string Content { get; set; }
	public string CategoryId { get; set; }
	public string CategoryContent { get; set; }
	public string SubcategoryId { get; set; }
	public string SubcategoryContent { get; set; }
}
