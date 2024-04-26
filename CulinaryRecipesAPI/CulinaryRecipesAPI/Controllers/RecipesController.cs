using CulinaryRecipes.ApplicationServices.Abstractions;
using CulinaryRecipes.DataObjects;
using CulinaryRecipes.Domain;
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
		public async Task<ActionResult<Pagination<RecipesToReturn>>> GetRecipes([FromQuery]RecipeParameters param)
		{
			var recipes = await _recipeService.GetRecipes(param);
			var noRecipes = await _recipeService.GetNumberOfRecipes();
			return Ok(new Pagination<RecipesToReturn>(param.PageNumber, param.PageSize, noRecipes, recipes));
		}

        [HttpGet("search/{name}")]
        public async Task<ActionResult<Pagination<RecipesToReturn>>> GetRecipesByName([FromRoute]string name,
            [FromQuery] RecipeParameters param)
        {
            var recipes = await _recipeService.GetRecipesByName(name, param);
            var noRecipes = await _recipeService.GetNumberOfRecipesByName(name);
            return Ok(new Pagination<RecipesToReturn>(param.PageNumber, param.PageSize, noRecipes, recipes));
        }

        [HttpGet("search/ingredients")]
        public async Task<ActionResult<Pagination<RecipesToReturn>>> GetRecipesByIngredients(
            [FromQuery]string[] selectedIngredients, [FromQuery] RecipeParameters param)
        {
            var recipes = await _recipeService.GetRecipesByIngredients(selectedIngredients, param);
            var noRecipes = await _recipeService.GetNumberOfRecipesByIngredients(selectedIngredients);
            return Ok(new Pagination<RecipesToReturn>(param.PageNumber, param.PageSize, noRecipes, recipes));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RecipeToReturn>> GetRecipeById(string id)
        {
            var recipe = await _recipeService.GetRecipeById(id);
            return Ok(recipe);
        }
	}
}
