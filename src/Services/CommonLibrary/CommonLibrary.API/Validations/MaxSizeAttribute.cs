using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CommonLibrary.API.Validations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
public class MaxSizeAttribute : ValidationAttribute
{
	private static readonly string[] suffixes = { "B", "KB", "MB", "GB", "TB" };
	private readonly long _minBytes;
	private readonly long _maxBytes;

	public MaxSizeAttribute(long minBytes, long maxBytes)
	{
		_minBytes = minBytes;
		_maxBytes = maxBytes;
	}

	protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
	{
		var casted = (IFormFile)value!;

		if (casted != null)
		{
			if (!(_minBytes < casted.Length && casted.Length < _maxBytes))
			{
				var current = ConvertToReadableSize(casted.Length);
				var min = ConvertToReadableSize(_minBytes);
				var max = ConvertToReadableSize(_maxBytes);
				return new ValidationResult($"Out of size ({current}). Must between {min} and {max}.");
			}
		}

		return ValidationResult.Success;
	}

	private static string ConvertToReadableSize(long bytes)
	{
		if (bytes == 0) return "0" + suffixes[0];
		long absBytes = Math.Abs(bytes);
		int place = Convert.ToInt32(Math.Floor(Math.Log(absBytes, 1024)));
		double num = Math.Round(absBytes / Math.Pow(1024, place), 1);
		return $"{Math.Sign(bytes) * num}{suffixes[place]}";
	}
}
