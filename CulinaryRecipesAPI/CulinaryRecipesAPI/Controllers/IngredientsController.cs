using CulinaryRecipes.ApplicationServices.Abstractions;
using CulinaryRecipes.Domain;
using CulinaryRecipes.Domain.Specifications;
using CulinaryRecipesAPI.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace CulinaryRecipesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientsController : ControllerBase
    {
        private readonly IIngredientService _ingredientService;

        public IngredientsController(IIngredientService ingredientService)
        {
            _ingredientService = ingredientService;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<Ingredient>>> GetIngredients([FromQuery]IngredientParameters param)
        {
            var ingredients = await _ingredientService.GetIngredients(param);
            var noIngredients = await _ingredientService.GetNumberOfIngredients(param.Name);
            return Ok(new Pagination<Ingredient>(param.PageNumber, param.PageSize, noIngredients, ingredients));
        }
    }
}
