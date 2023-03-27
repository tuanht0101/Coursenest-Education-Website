using System.ComponentModel.DataAnnotations;

namespace Authentication.API.Options;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class JwtOptions
{
	[Required]
	[RegularExpression(@"^[a-zA-Z0-9]{8,32}$",
		ErrorMessage = "Value for {0} must be ^[a-zA-Z0-9]{8,32}$.")]
	public string SigningKey { get; set; }

	public string Issuer { get; set; }

	[Range(1, int.MaxValue,
		ErrorMessage = "Value for {0} must be bigger than or equal to {1}.")]
	public int AccessTokenLifetime { get; set; } = 10;

	[Range(1, int.MaxValue,
		ErrorMessage = "Value for {0} must be bigger than or equal to {1}.")]
	public int RefreshTokenLifetime { get; set; } = 30;
}
