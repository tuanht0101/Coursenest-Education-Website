using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace CommonLibrary.Tests;
internal class TestAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
	public const string SchemeName = "TestScheme";

	public TestAuthenticationHandler(
		IOptionsMonitor<AuthenticationSchemeOptions> options,
		ILoggerFactory logger,
		UrlEncoder encoder,
		ISystemClock clock)
		: base(options, logger, encoder, clock)
	{
	}

	protected override Task<AuthenticateResult> HandleAuthenticateAsync()
	{
		var claims = new List<Claim>();

		if (Context.Request.Headers.TryGetValue(nameof(ClaimTypes.NameIdentifier), out var nameIdentifiers))
		{
			var id = nameIdentifiers[0];
			if (!string.IsNullOrWhiteSpace(id))
				claims.Add(new Claim(ClaimTypes.NameIdentifier, id));
		}

		if (Context.Request.Headers.TryGetValue(nameof(ClaimTypes.Role), out var roles))
		{
			foreach (var role in roles)
			{
				if (!string.IsNullOrWhiteSpace(role))
					claims.Add(new Claim(ClaimTypes.Role, role));
			}
		}

		var identity = new ClaimsIdentity(claims, "Test");
		var principal = new ClaimsPrincipal(identity);
		var ticket = new AuthenticationTicket(principal, SchemeName);

		var result = AuthenticateResult.Success(ticket);

		return Task.FromResult(result);
	}
}
