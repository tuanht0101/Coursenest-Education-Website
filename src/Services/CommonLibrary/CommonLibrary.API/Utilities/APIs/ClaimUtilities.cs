using CommonLibrary.API.Models;
using System.Security.Claims;

namespace CommonLibrary.API.Utilities.APIs;
public static class ClaimUtilities
{
	public static int GetUserId(IEnumerable<Claim> claims)
	{
		var value = claims
			.First(x => x.Type == ClaimTypes.NameIdentifier)
			.Value;

		return int.Parse(value);
	}

	public static IEnumerable<RoleType> GetRoles(IEnumerable<Claim> claims)
	{
		var types = claims
			.Where(x => x.Type == ClaimTypes.Role)
			.Select(x => Enum.Parse<RoleType>(x.Value));

		return types;
	}
}
