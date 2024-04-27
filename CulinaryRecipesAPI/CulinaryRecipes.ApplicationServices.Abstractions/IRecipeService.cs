using CulinaryRecipes.DataObjects;
using CulinaryRecipes.Domain;
using CulinaryRecipes.Domain.Specifications;

namespace CulinaryRecipes.ApplicationServices.Abstractions
{
	public interface IRecipeService
	{
		Task<List<RecipesToReturn>> GetRecipes(RecipeParameters param);
        Task<List<RecipesToReturn>> GetRecipesByName(string name, RecipeParameters param);
        Task<List<RecipesToReturn>> GetRecipesByIngredients(string[] selectedIngredients, RecipeParameters param);
        Task<List<RecipesToReturn>> GetRecipesByAuthor(string authorName, string clickedRecipeId, RecipeParameters param);
        Task<int> GetNumberOfRecipes();
        Task<int> GetNumberOfRecipesByName(string name);
        Task<int> GetNumberOfRecipesByIngredients(string[] selectedIngredients);
        Task<int> GetNumberOfRecipesByAuthor(string authorName);
        Task<RecipeToReturn> GetRecipeById(string id);

    }
}
