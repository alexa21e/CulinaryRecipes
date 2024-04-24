using CulinaryRecipes.DataObjects;
using CulinaryRecipes.Domain;
using CulinaryRecipes.Domain.Specifications;

namespace CulinaryRecipes.ApplicationServices.Abstractions
{
	public interface IRecipeService
	{
		Task<List<RecipesToReturn>> GetRecipes(RecipeParameters param);
		Task<int> GetNumberOfRecipes();
        Task<RecipeToReturn> GetRecipeById(string id);

    }
}
