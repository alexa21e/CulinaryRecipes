using CulinaryRecipes.DataObjects;
using CulinaryRecipes.Domain.Specifications;

namespace CulinaryRecipes.ApplicationServices.Abstractions
{
	public interface IRecipeService
    {
        Task<List<HomeRecipeToReturn>> GetRecipes(RecipeParameters param);
        Task<int> GetRecipesCount(RecipeParameters param);
        Task<List<HomeRecipeToReturn>> GetRecipesByAuthor(string authorName, string clickedRecipeId, RecipeParameters param);
        Task<List<HomeRecipeToReturn>> GetRecipesByAuthorAndName(string authorName, string clickedRecipeId, string recipeName, RecipeParameters param);
        Task<List<HomeRecipeToReturn>> GetRecipesByAuthorAndIngredients(string authorName, string clickedRecipeId, string selectedIngredients,  RecipeParameters param);
        Task<List<RecipeStatsToReturn>> GetMostComplexRecipes(int recipesNumber);
        Task<int> GetNumberOfRecipesByAuthor(string authorName);
        Task<int> GetNumberOfRecipesByAuthorAndName(string authorName, string recipeName);
        Task<int> GetNumberOfRecipesByAuthorAndIngredients(string authorName, string selectedIngredients);
        Task<DetailedRecipeToReturn> GetRecipeById(string id);
        Task<List<SimilarRecipeToReturn>> GetFiveMostSimilarRecipes(string id);
    }
}
