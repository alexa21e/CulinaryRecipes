using CulinaryRecipes.DataObjects;
using CulinaryRecipes.Domain;

namespace CulinaryRecipes.DataAccess.Abstractions
{
	public interface IRecipeRepository
	{
		Task<List<RecipesToReturn>> GetRecipes(int skip, int pageSize);
        Task<List<RecipesToReturn>> GetRecipesByName(string name, int skip, int pageSize);
		Task<int> GetNumberOfRecipes();
        Task<int> GetNumberOfRecipesByName(string name);
        Task<RecipeToReturn> GetRecipeById(string id);
    }
}
