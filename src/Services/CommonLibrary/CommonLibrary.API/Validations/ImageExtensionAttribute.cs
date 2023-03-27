using CommonLibrary.API.Constants;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CommonLibrary.API.Validations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
public class ImageExtensionAttribute : ValidationAttribute
{
	protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
	{
		var casted = (IFormFile)value!;

		if (casted != null)
		{
			var extensions = FormFileContants.ExtensionMIMEs.Keys;

			if (!extensions.Contains(Path.GetExtension(casted.FileName).ToLowerInvariant()))
			{
				return new ValidationResult($"Unsupported extension {casted}. Must be {string.Join(", ", extensions)}");
			}
		}

		return ValidationResult.Success;
	}
}
