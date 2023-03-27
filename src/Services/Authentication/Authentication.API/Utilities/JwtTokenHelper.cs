using Authentication.API.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authentication.API.Utilities;

public class JwtTokenHelper
{
	private readonly IOptionsMonitor<JwtOptions> _options;
	private SigningCredentials _signingCredentials;

	public JwtTokenHelper(IOptionsMonitor<JwtOptions> options)
	{
		_options = options;
		_options.OnChange((options) =>
		{
			_signingCredentials = CreateSigningCredentials(options.SigningKey);
		});

		_signingCredentials = CreateSigningCredentials(_options.CurrentValue.SigningKey);
	}


	private static SigningCredentials CreateSigningCredentials(string signingKey)
	{
		var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
		return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
	}

	public string CreateToken(IEnumerable<Claim>? claims, DateTime expiry)
	{
		var securityToken = new JwtSecurityToken(
			issuer: _options.CurrentValue.Issuer,
			claims: claims,
			expires: expiry,
			signingCredentials: _signingCredentials
		);

		var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

		return token;
	}
}
