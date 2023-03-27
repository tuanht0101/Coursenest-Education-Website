using System.ComponentModel.DataAnnotations;

namespace Library.API.DTOs.Units;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record CreateQuestion : IValidatableObject
{
	public string Content { get; set; }
	public int Point { get; set; }
	public int ExamUnitId { get; set; }
	public List<CreateChoice> Choices { get; set; }

	public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
	{
		if (Choices.Count == 1)
		{
			yield return new ValidationResult($"Must have only either 0 or more than 1 Choice.");
		}
	}

	public record CreateChoice
	{
		public string Content { get; set; }
		public bool IsCorrect { get; set; }
	}
}
