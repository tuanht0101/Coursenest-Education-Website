using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.API.Extensions;
public static class AppExtensions
{
	public static IApplicationBuilder UseSwaggerUI(this IApplicationBuilder app)
	{
		var configuration = app.ApplicationServices.GetRequiredService<IConfiguration>();
		if (configuration["SwaggerUI"] == "True")
		{
			app.UseSwagger();
			SwaggerUIBuilderExtensions.UseSwaggerUI(app);
		}
		
		return app;
	}
}
