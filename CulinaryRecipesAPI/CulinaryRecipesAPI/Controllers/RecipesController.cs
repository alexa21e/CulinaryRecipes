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
        public async Task<ActionResult<Pagination<HomeRecipeToReturn>>> GetRecipes([FromQuery] RecipeParameters param)
        {
            var recipes = await _recipeService.GetRecipes(param);
            var noRecipes = await _recipeService.GetRecipesCount(param);
            return Ok(new Pagination<HomeRecipeToReturn>(param.PageNumber, param.PageSize, noRecipes, param.SortOrder, recipes));
        }

        [HttpGet("author")]
        public async Task<ActionResult<Pagination<HomeRecipeToReturn>>> GetRecipesByAuthor([FromQuery] AuthorRecipeParameters param)
        {
            var recipes = await _recipeService.GetRecipesByAuthor(param);
            var noRecipes = await _recipeService.GetRecipesByAuthorCount(param);
            return Ok(new Pagination<HomeRecipeToReturn>(param.PageNumber, param.PageSize, noRecipes, param.SortOrder, recipes));
        }

        [HttpGet("mostcomplex")]
        public async Task<ActionResult<List<RecipeStatsToReturn>>> GetMostComplexRecipes([FromQuery] int recipesNumber)
        {
            var recipes = await _recipeService.GetMostComplexRecipes(recipesNumber);
            return Ok(recipes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DetailedRecipeToReturn>> GetRecipeById([FromRoute] string id)
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
