using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;

namespace CommonLibrary.API.Extensions;
public static class BuilderExtensions
{
	private static void AddMassTransitServices(
		this IServiceCollection services,
		IConfiguration configuration,
		Assembly assembly)
	{
		var massTransitHost = configuration["MassTransit:Host"];
		if (!string.IsNullOrWhiteSpace(massTransitHost))
		{
			services.AddMassTransit(bus =>
			{
				bus.UsingRabbitMq((context, config) =>
				{
					config.Host(massTransitHost);
					config.ConfigureEndpoints(context);
				});

				bus.AddConsumers(assembly);
			});
		}
	}

	private static void AddJWTAuthentication(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters()
				{
					ValidateIssuer = false,
					ValidateAudience = false,
					ValidateLifetime = false,
					ValidateIssuerSigningKey = false,
					RequireSignedTokens = false,
				};

				var issuer = configuration["Jwt:Issuer"];
				if (!string.IsNullOrWhiteSpace(issuer))
				{
					options.TokenValidationParameters.ValidateIssuer = true;
					options.TokenValidationParameters.ValidIssuer = issuer;
				}

				var signingKey = configuration["Jwt:SigningKey"];
				if (!string.IsNullOrWhiteSpace(signingKey))
				{
					options.TokenValidationParameters.ValidateLifetime = true;
					options.TokenValidationParameters.ValidateIssuerSigningKey = true;
					options.TokenValidationParameters.RequireSignedTokens = true;
					options.TokenValidationParameters.IssuerSigningKey =
						new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
				}
				else
				{
					options.TokenValidationParameters.SignatureValidator = delegate (string token, TokenValidationParameters parameters)
					{
						return new JwtSecurityToken(token);
					};
				}
			});
	}

	private static void AddSwagger(
		this IServiceCollection services)
	{
		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		services.AddEndpointsApiExplorer();

		services.AddSwaggerGen(options =>
		{
			options.AddSecurityDefinition(
				JwtBearerDefaults.AuthenticationScheme,
				new OpenApiSecurityScheme()
				{
					Name = "Authorization",
					Description = "Token here",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.Http,
					Scheme = JwtBearerDefaults.AuthenticationScheme
				});

			options.AddSecurityRequirement(new OpenApiSecurityRequirement()
			{
				{
					new OpenApiSecurityScheme()
					{
						Reference = new OpenApiReference
						{
							Type = ReferenceType.SecurityScheme,
							Id = JwtBearerDefaults.AuthenticationScheme
						}
					},
					Array.Empty<string>()
				}
			});
		});
	}

	private static void AddEFCoreServices<TDbContext>(
		this IServiceCollection services,
		IConfiguration configuration) where TDbContext : DbContext
	{
		var cnnStr = configuration["Database:ConnectionString"];
		if (!string.IsNullOrWhiteSpace(cnnStr))
		{
			services.AddDbContext<DbContext, TDbContext>(builder =>
			{
				builder.UseSqlServer(cnnStr, builder =>
				{
					//builder.EnableRetryOnFailure(1, TimeSpan.FromSeconds(3), null);
				});
			});
		}
	}

	private static void AddEssentialServices(
		this IServiceCollection services,
		IConfiguration configuration,
		Assembly assembly)
	{
		services.AddControllers();

		services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

		//services.AddCors(options =>
		//{
		//	options.AddDefaultPolicy(configure =>
		//	{
		//		configure.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
		//	});
		//});

		services.AddAutoMapper(assembly);

		services.AddMassTransitServices(configuration, assembly);

		services.AddJWTAuthentication(configuration);

		services.AddAuthorization();

		services.AddSwagger();
	}


	public static IServiceCollection AddDefaultServices(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		var assembly = Assembly.GetCallingAssembly();
		services.AddEssentialServices(configuration, assembly);

		return services;
	}

	public static IServiceCollection AddDefaultServices<TDbContext>(
		this IServiceCollection services,
		IConfiguration configuration) where TDbContext : DbContext
	{
		var assembly = Assembly.GetCallingAssembly();
		services.AddEssentialServices(configuration, assembly);

		services.AddEFCoreServices<TDbContext>(configuration);

		return services;
	}
}
