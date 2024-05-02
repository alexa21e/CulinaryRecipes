using CulinaryRecipes.ApplicationServices.Abstractions;
using CulinaryRecipes.DataObjects;
using CulinaryRecipes.Domain.Specifications;
using CulinaryRecipesAPI.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace CulinaryRecipesAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class RecipesController : ControllerBase
	{
		private readonly IRecipeService _recipeService;

		public RecipesController(IRecipeService recipeService)
		{
			_recipeService = recipeService;
		}

		[HttpGet]
		public async Task<ActionResult<Pagination<RecipesToReturn>>> GetRecipes([FromQuery] RecipeParameters param)
		{
			var recipes = await _recipeService.GetRecipes(param);
			var noRecipes = await _recipeService.GetNumberOfRecipes();
			return Ok(new Pagination<RecipesToReturn>(param.PageNumber, param.PageSize, noRecipes, param.SortOrder, recipes));
		}

        [HttpGet("search/name")]
        public async Task<ActionResult<Pagination<RecipesToReturn>>> GetRecipesByName([FromQuery] string name,
            [FromQuery] RecipeParameters param)
        {
            var recipes = await _recipeService.GetRecipesByName(name, param);
            var noRecipes = await _recipeService.GetNumberOfRecipesByName(name);
            return Ok(new Pagination<RecipesToReturn>(param.PageNumber, param.PageSize, noRecipes, param.SortOrder, recipes));
        }

        [HttpGet("search/ingredients")]
        public async Task<ActionResult<Pagination<RecipesToReturn>>> GetRecipesByIngredients(
            [FromQuery] string selectedIngredients, [FromQuery] RecipeParameters param)
        {
            var ingredients = selectedIngredients.Split(',');
            var recipes = await _recipeService.GetRecipesByIngredients(ingredients, param);
            var noRecipes = await _recipeService.GetNumberOfRecipesByIngredients(ingredients);
            return Ok(new Pagination<RecipesToReturn>(param.PageNumber, param.PageSize, noRecipes, param.SortOrder, recipes));
        }

        [HttpGet("author/{authorName}/clickedRecipe/{clickedRecipeId}")]
        public async Task<ActionResult<Pagination<RecipesToReturn>>>
            GetRecipesByAuthor([FromRoute] string authorName, [FromRoute] string clickedRecipeId, [FromQuery] RecipeParameters param)
        {
            var recipes = await _recipeService.GetRecipesByAuthor(authorName, clickedRecipeId, param);
            var noRecipes = await _recipeService.GetNumberOfRecipesByAuthor(authorName);
            return Ok(new Pagination<RecipesToReturn>(param.PageNumber, param.PageSize, noRecipes, param.SortOrder, recipes));
        }

        [HttpGet("author/{authorName}/clickedRecipe/{clickedRecipeId}/search/name")]
        public async Task<ActionResult<Pagination<RecipesToReturn>>>
            GetRecipesByAuthorAndName([FromRoute] string clickedRecipeId, [FromRoute] string authorName, [FromQuery] string recipeName,
                [FromQuery] RecipeParameters param)
        {
            var recipes = await _recipeService.GetRecipesByAuthorAndName(authorName, clickedRecipeId, recipeName, param);
            var noRecipes = await _recipeService.GetNumberOfRecipesByAuthorAndName(authorName, recipeName);
            return Ok(new Pagination<RecipesToReturn>(param.PageNumber, param.PageSize, noRecipes, param.SortOrder, recipes));
        }

        [HttpGet("author/{authorName}/clickedRecipe/{clickedRecipeId}/search/ingredients")]

        public async Task<ActionResult<Pagination<RecipesToReturn>>>
            GetRecipesByAuthorAndIngredients([FromRoute] string authorName, [FromRoute] string clickedRecipeId, 
                [FromQuery] string selectedIngredients, [FromQuery] RecipeParameters param)
        {
            var recipes = await _recipeService.GetRecipesByAuthorAndIngredients(authorName, clickedRecipeId, selectedIngredients, param);
            var noRecipes = await _recipeService.GetNumberOfRecipesByAuthorAndIngredients(authorName, selectedIngredients);
            return Ok(new Pagination<RecipesToReturn>(param.PageNumber, param.PageSize, noRecipes, param.SortOrder, recipes));
        }

        [HttpGet("mostcomplex")]
        public async Task<ActionResult<List<RecipeStatsToReturn>>> GetMostComplexRecipes([FromQuery] int recipesNumber)
        {
            var recipes = await _recipeService.GetMostComplexRecipes(recipesNumber);
            return Ok(recipes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RecipeToReturn>> GetRecipeById([FromRoute] string id)
        {
            var recipe = await _recipeService.GetRecipeById(id);
            return Ok(recipe);
        }

        [HttpGet("{id}/similar")]
        public async Task<ActionResult<List<SimilarRecipeToReturn>>> GetFiveMostSimilarRecipes([FromRoute] string id)
        {
            var recipes = await _recipeService.GetFiveMostSimilarRecipes(id);
            return Ok(recipes);
        }
	}
}
