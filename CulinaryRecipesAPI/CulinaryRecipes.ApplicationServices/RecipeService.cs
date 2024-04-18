using CulinaryRecipes.ApplicationServices.Abstractions;
using CulinaryRecipes.DataAccess.Abstractions;
using CulinaryRecipes.Domain;

namespace CulinaryRecipes.ApplicationServices
{
	public class RecipeService: IRecipeService
	{
		private readonly IRecipeRepository _recipeRepository;

		public RecipeService(IRecipeRepository recipeRepository)
		{
			_recipeRepository = recipeRepository;
		}

		public async Task<List<Dictionary<string, object>>> GetRecipes(int pageNumber, int pageSize)
		{
			var skip = (pageNumber - 1) * pageSize;
			return await _recipeRepository.GetRecipes(skip, pageSize);
		}
	}
}
