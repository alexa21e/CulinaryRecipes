using CulinaryRecipes.DataObjects;
using CulinaryRecipes.Domain;

namespace CulinaryRecipes.DataAccess.Abstractions
{
	public interface IRecipeRepository
    {
        Task<List<HomeRecipeToReturn>> GetRecipes(int skip, int pageSize, string sortOrder,
            string? name, string[]? selectedIngredients);
        Task<int> GetRecipesCount(string? name, string[]? selectedIngredients);
        Task<List<HomeRecipeToReturn>> GetRecipesByAuthor(string authorName, int skip, int pageSize, string sortOrder);
        Task<List<HomeRecipeToReturn>> GetRecipesByAuthorAndName(string authorName, string recipeName, int skip,
            int pageSize, string sortOrder);
        Task<List<HomeRecipeToReturn>> GetRecipesByAuthorAndIngredients(string authorName,
            string[] selectedIngredients, int skip, int pageSize, string sortOrder);
        Task<List<RecipeStatsToReturn>> GetMostComplexRecipes(int recipesNumber);
        Task<int> GetNumberOfRecipesByAuthor(string authorName);
        Task<int> GetNumberOfRecipesByAuthorAndName(string authorName, string recipeName);
        Task<int> GetNumberOfRecipesByAuthorAndIngredients(string authorName, string[] selectedIngredients);
        Task<DetailedRecipeToReturn> GetRecipeById(string id);
        Task<List<SimilarRecipeToReturn>> GetFiveMostSimilarRecipes(string id);
    }
}
