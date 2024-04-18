using CulinaryRecipes.ApplicationServices.Abstractions;
using CulinaryRecipes.DataAccess.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace CulinaryRecipesAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class RecipesController : ControllerBase
	{
		private readonly IRecipeService _recipeService;

		public RecipesController(IRecipeService recipeService)
		{
			_recipeService = recipeService;
		}

		[HttpGet]
		public async Task<IActionResult> GetRecipes(int pageNumber, int pageSize)
		{
			var recipes = await _recipeService.GetRecipes(pageNumber, pageSize);
			return Ok(recipes);
		}
	}
}
