using CulinaryRecipes.DataObjects;
using CulinaryRecipes.Domain.Specifications;

namespace CulinaryRecipes.ApplicationServices.Abstractions
{
	public interface IRecipeService
    {
        Task<List<HomeRecipeToReturn>> GetRecipes(RecipeParameters param);
        Task<int> GetRecipesCount(RecipeParameters param);
        Task<List<HomeRecipeToReturn>> GetRecipesByAuthor(AuthorRecipeParameters param);
        Task<int> GetRecipesByAuthorCount(AuthorRecipeParameters param);
        Task<List<RecipeStatsToReturn>> GetMostComplexRecipes(int recipesNumber);
        Task<DetailedRecipeToReturn> GetRecipeById(string id);
        Task<List<SimilarRecipeToReturn>> GetFiveMostSimilarRecipes(string id);
        Task<RecipeNameToReturn> GetRecipeNameById(string id);
    }
}
