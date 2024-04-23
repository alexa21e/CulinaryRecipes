using CulinaryRecipes.DataObjects;
using CulinaryRecipes.Domain;

namespace CulinaryRecipes.DataAccess.Abstractions
{
	public interface IRecipeRepository
	{
		Task<List<Dictionary<string, object>>> GetRecipes(int skip, int pageSize);
		Task<int> GetNumberOfRecipes();
        Task<Recipe> GetRecipeById(string id);

    }
}
