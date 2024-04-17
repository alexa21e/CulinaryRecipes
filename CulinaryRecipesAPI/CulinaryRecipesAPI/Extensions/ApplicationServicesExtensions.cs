using CulinaryRecipes.ApplicationServices;
using CulinaryRecipes.ApplicationServices.Abstractions;
using CulinaryRecipes.DataAccess;
using CulinaryRecipes.DataAccess.Abstractions;
using Microsoft.OpenApi.Models;
using Neo4jClient;

namespace CulinaryRecipesAPI.Extensions
{
	public static class ApplicationServicesExtensions
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services,
			IConfiguration config)
		{
			services.AddSingleton<IGraphClient>(x =>
			{
				var neo4jConfig = config.GetSection("Neo4j");
				var client = new BoltGraphClient(neo4jConfig["Url"], neo4jConfig["Username"], neo4jConfig["Password"]);
				client.ConnectAsync().Wait();
				return client;
			});

			services.AddCors(options =>
			{
				options.AddPolicy(name: "AllowFrontEnd",
					policy =>
					{
						policy.WithOrigins("http://localhost:4200")
							.AllowAnyHeader()
							.AllowAnyMethod();
					});
			});

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "CulinaryRecipes", Version = "v1" });
			});

			services.AddScoped<IRecipeRepository, RecipeRepository>();
			services.AddScoped<IRecipeService, RecipeService>();

			return services;
		}
	}
}
