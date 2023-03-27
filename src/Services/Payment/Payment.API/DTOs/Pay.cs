using CommonLibrary.API.Models;
using System.ComponentModel.DataAnnotations;

namespace Payment.API.DTOs;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record Pay
{
	public int CardNumber { get; set; }

	[RegularExpression(@"^[0-9]{3}$")]
	public string CVV { get; set; }

	[Range(1, 12)]
	public int ExpiryDateMonth { get; set; }

	[Range(0, 99)]
	public int ExpiryDateYear { get; set; }

	public RoleType Role { get; set; }

	[Range(1, 36)]
	public int Months { get; set; }
}
