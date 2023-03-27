using Authentication.API.Infrastructure.Contexts;
using Authentication.API.Options;
using Authentication.API.Utilities;
using CommonLibrary.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDefaultServices<DataContext>(builder.Configuration);

var section = builder.Configuration.GetSection("Jwt");
if (section.Exists())
{
	builder.Services
		.AddOptions<JwtOptions>()
		.Bind(section)
		.ValidateDataAnnotations()
		.ValidateOnStart();

	builder.Services.AddSingleton<JwtTokenHelper>();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwaggerUI();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
