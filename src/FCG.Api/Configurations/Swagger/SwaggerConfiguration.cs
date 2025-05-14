using System.Reflection;

namespace FCG.Api.Configurations.Swagger {

	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.OpenApi.Models;

	public static class SwaggerConfiguration
	{
		public static void AddSwaggerServices(this IServiceCollection services)
		{
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "FCG API", Version = "v1" });
				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"; // Usando o nome do assembly
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				c.IncludeXmlComments(xmlPath); // Inclua comentários XML se necessário
				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Description = "JWT no padrão **Bearer {token}**",
					Name = "Authorization",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.Http,
					Scheme = "Bearer",
					BearerFormat = "JWT",
				});
				
				c.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new  OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Id = "Bearer",
								Type = ReferenceType.SecurityScheme
							}	
						},
						Array.Empty<string>()
					}
				});
			});
		}

		public static void UseSwaggerSetup(this IApplicationBuilder app)
		{
			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "FCG API V1");
				c.RoutePrefix = string.Empty;
			});
		}
	}

}