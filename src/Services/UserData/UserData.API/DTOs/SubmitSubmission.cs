using System.ComponentModel.DataAnnotations;

namespace UserData.API.DTOs;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record SubmitSubmission
{
	public Answer[] Answers { get; set; }

	public record Answer : IValidatableObject
	{
		public int QuestionId { get; set; }
		public int? ChoiceId { get; set; }
		public string? Content { get; set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if ((ChoiceId == null && Content == null) || (ChoiceId != null && Content != null))
			{
				yield return new ValidationResult($"Must be only either ChoiceId or Content allowed.");
			}
		}
	}
}
