using CulinaryRecipes.DataObjects;
using CulinaryRecipes.Domain.Specifications;

namespace CulinaryRecipes.ApplicationServices.Abstractions
{
	public interface IRecipeService
	{
		Task<List<RecipesToReturn>> GetRecipes(RecipeParameters param);
        Task<List<RecipesToReturn>> GetRecipesByName(string name, RecipeParameters param);
        Task<List<RecipesToReturn>> GetRecipesByIngredients(string[] selectedIngredients, RecipeParameters param);
        Task<List<RecipesToReturn>> GetRecipesByAuthor(string authorName, string clickedRecipeId, RecipeParameters param);
        Task<List<RecipesToReturn>> GetRecipesByAuthorAndName(string authorName, string clickedRecipeId, string recipeName, RecipeParameters param);
        Task<List<RecipesToReturn>> GetRecipesByAuthorAndIngredients(string authorName, string clickedRecipeId, string selectedIngredients,  RecipeParameters param);
        Task<List<RecipeStatsToReturn>> GetMostComplexRecipes(int recipesNumber);
        Task<int> GetNumberOfRecipes();
        Task<int> GetNumberOfRecipesByName(string name);
        Task<int> GetNumberOfRecipesByIngredients(string[] selectedIngredients);
        Task<int> GetNumberOfRecipesByAuthor(string authorName);
        Task<int> GetNumberOfRecipesByAuthorAndName(string authorName, string recipeName);
        Task<int> GetNumberOfRecipesByAuthorAndIngredients(string authorName, string selectedIngredients);
        Task<RecipeToReturn> GetRecipeById(string id);
        Task<List<SimilarRecipeToReturn>> GetFiveMostSimilarRecipes(string id);
    }
}
