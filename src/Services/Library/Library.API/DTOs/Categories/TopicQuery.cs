using System.ComponentModel.DataAnnotations;

namespace Library.API.DTOs.Categories;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record TopicQuery : IValidatableObject
{
	public string? Content { get; set; }
	public int? SubcategoryId { get; set; }

	public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
	{
		if (Content == null && SubcategoryId == null)
			yield return new ValidationResult($"Must have either Content or SubcategoryId or both.");
	}
}
