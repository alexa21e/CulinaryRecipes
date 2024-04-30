using CulinaryRecipes.DataObjects;
using CulinaryRecipes.Domain;

namespace CulinaryRecipes.DataAccess.Abstractions
{
	public interface IRecipeRepository
	{
		Task<List<RecipesToReturn>> GetRecipes(int skip, int pageSize, string sortOrder);
        Task<List<RecipesToReturn>> GetRecipesByName(string name, int skip, int pageSize, string sortOrder);
        Task<List<RecipesToReturn>> GetRecipesByIngredients(string[] selectedIngredients, int skip, int pageSize, string sortOrder);
        Task<List<RecipesToReturn>> GetRecipesByAuthor(string authorName, int skip, int pageSize, string sortOrder);
        Task<List<RecipesToReturn>> GetRecipesByAuthorAndName(string authorName, string recipeName, int skip,
            int pageSize, string sortOrder);
        Task<List<RecipesToReturn>> GetRecipesByAuthorAndIngredients(string authorName,
            string[] selectedIngredients, int skip, int pageSize, string sortOrder);
        Task<int> GetNumberOfRecipes();
        Task<int> GetNumberOfRecipesByName(string name);
        Task<int> GetNumberOfRecipesByIngredients(string[] selectedIngredients);
        Task<int> GetNumberOfRecipesByAuthor(string authorName);
        Task<int> GetNumberOfRecipesByAuthorAndName(string authorName, string recipeName);
        Task<int> GetNumberOfRecipesByAuthorAndIngredients(string authorName, string[] selectedIngredients);
        Task<RecipeToReturn> GetRecipeById(string id);
    }
}
