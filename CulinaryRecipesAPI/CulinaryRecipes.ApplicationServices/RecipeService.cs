using CulinaryRecipes.ApplicationServices.Abstractions;
using CulinaryRecipes.DataAccess.Abstractions;
using CulinaryRecipes.DataObjects;
using CulinaryRecipes.Domain;
using CulinaryRecipes.Domain.Specifications;

namespace CulinaryRecipes.ApplicationServices
{
	public class RecipeService: IRecipeService
	{
		private readonly IRecipeRepository _recipeRepository;

		public RecipeService(IRecipeRepository recipeRepository)
		{
			_recipeRepository = recipeRepository;
		}

		public async Task<List<Dictionary<string, object>>> GetRecipes(RecipeParameters param)
		{
			var skip = (param.PageNumber - 1) * param.PageSize;
			return await _recipeRepository.GetRecipes(skip, param.PageSize);
		}

		public async Task<int> GetNumberOfRecipes()
		{
			return await _recipeRepository.GetNumberOfRecipes();
		}

        public async Task<Recipe> GetRecipeById(string id)
        {
            return await _recipeRepository.GetRecipeById(id);
        }
	}
}
