using Authentication.API.Infrastructure.Contexts;
using Authentication.API.Infrastructure.Entities;
using CommonLibrary.API.Models;

namespace Authentication.Tests;

public static class Defaults
{
	public static readonly Action<DataContext> Database = context =>
	{
		var expiry = DateTime.Now.AddHours(1);

		context.Credentials.AddRange(new[]
		{
			new Credential()
			{
				UserId = 1,
				Username = "loginTest",
				Password = "pwd",
				Roles = new()
				{
					new()
					{
						Type = RoleType.Student,
						Expiry = expiry
					},
					new()
					{
						Type = RoleType.Instructor,
						Expiry = expiry
					}
				}
			},
			new Credential()
			{
				UserId = 2,
				Username = "logoutTest",
				Password = "pwd",
				RefreshTokens = new()
				{
					new()
					{
						Token = "logoutTestRT",
						Expiry = expiry
					}
				}
			},
			new Credential()
			{
				UserId = 3,
				Username = "refreshTest",
				Password = "pwd",
				RefreshTokens = new()
				{
					new()
					{
						Token = "refreshTestRT",
						Expiry = expiry
					}
				}
			},
			new Credential()
			{
				UserId = 4,
				Username = "resetTest",
				Password = "pwd"
			},
			new Credential()
			{
				UserId = 5,
				Username = "forgotTest",
				Password = "pwd"
			},
			new Credential()
			{
				UserId = 6,
				Username = "changeTest",
				Password = "pwd"
			}
		});
	};
}
