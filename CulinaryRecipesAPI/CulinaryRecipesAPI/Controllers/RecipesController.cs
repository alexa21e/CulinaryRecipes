using CulinaryRecipes.ApplicationServices.Abstractions;
using CulinaryRecipes.Domain.Specifications;
using CulinaryRecipesAPI.Helpers;
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
		public async Task<ActionResult<Pagination<Dictionary<string, object>>>> GetRecipes([FromQuery]RecipeParameters param)
		{
			var recipes = await _recipeService.GetRecipes(param);
			var noRecipes = await _recipeService.GetNumberOfRecipes();
			return Ok(new Pagination<Dictionary<string, object>>(param.PageNumber, param.PageSize, noRecipes, recipes));
		}
	}
}
