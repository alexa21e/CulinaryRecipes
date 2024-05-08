using CulinaryRecipes.ApplicationServices.Abstractions;
using CulinaryRecipes.DataAccess.Abstractions;
using CulinaryRecipes.DataObjects;
using CulinaryRecipes.Domain;
using CulinaryRecipes.Domain.Specifications;

namespace CulinaryRecipes.ApplicationServices
{
	public class RecipeService: IRecipeService
	{
		private readonly IRecipeRepository _recipeRepository;

		public RecipeService(IRecipeRepository recipeRepository)
		{
			_recipeRepository = recipeRepository;
		}

        public async Task<List<HomeRecipeToReturn>> GetRecipes(RecipeParameters param)
        {
            var skip = (param.PageNumber - 1) * param.PageSize;
            string[] ingredients = [];
            if (!string.IsNullOrEmpty(param.SelectedIngredients))
            {
                ingredients = param.SelectedIngredients.Split(',');
            }
            return await _recipeRepository.GetRecipes(skip, param.PageSize, param.SortOrder, param.RecipeName, ingredients);
        }

        public async Task<int> GetRecipesCount(RecipeParameters param)
        {
            string[] ingredients = [];
            if (!string.IsNullOrEmpty(param.SelectedIngredients))
            {
                ingredients = param.SelectedIngredients.Split(',');
            }
            return await _recipeRepository.GetRecipesCount(param.RecipeName, ingredients);
        }

        public async Task<List<HomeRecipeToReturn>> GetRecipesByAuthor(string authorName, string clickedRecipeId,
            RecipeParameters param)
        {
            var skip = (param.PageNumber - 1) * param.PageSize;
            var recipes = await _recipeRepository.GetRecipesByAuthor(authorName, skip, param.PageSize, param.SortOrder);
            recipes.RemoveAll(r => r.Id == clickedRecipeId);
            return recipes;
        }

        public async Task<List<HomeRecipeToReturn>> GetRecipesByAuthorAndName(string authorName, string clickedRecipeId, string recipeName,
            RecipeParameters param)
        {
            var skip = (param.PageNumber - 1) * param.PageSize;
            var recipes = await _recipeRepository.GetRecipesByAuthorAndName(authorName, recipeName, skip, param.PageSize,
                param.SortOrder);
            recipes.RemoveAll(r => r.Id == clickedRecipeId);
            return recipes;
        }

        public async Task<List<HomeRecipeToReturn>> GetRecipesByAuthorAndIngredients(string authorName, string clickedRecipeId,
            string selectedIngredients,  RecipeParameters param)
        {
            var skip = (param.PageNumber - 1) * param.PageSize;
            var ingredients = selectedIngredients.Split(',');
            var recipes = await _recipeRepository.GetRecipesByAuthorAndIngredients(authorName, ingredients, skip,
                param.PageSize, param.SortOrder);
            recipes.RemoveAll(r => r.Id == clickedRecipeId);
            return recipes;
        }

        public async Task<List<RecipeStatsToReturn>> GetMostComplexRecipes(int recipesNumber)
        {
            return await _recipeRepository.GetMostComplexRecipes(recipesNumber);
        }

        public async Task<int> GetNumberOfRecipesByAuthor(string authorName){
            var recipesNumber = await _recipeRepository.GetNumberOfRecipesByAuthor(authorName);
            return recipesNumber > 0 ? recipesNumber - 1 : 0;
        }

        public async Task<int> GetNumberOfRecipesByAuthorAndName(string authorName, string recipeName)
        {
            var recipesNumber = await _recipeRepository.GetNumberOfRecipesByAuthorAndName(authorName, recipeName);
            return recipesNumber > 0 ? recipesNumber - 1 : 0;
        }

        public async Task<int> GetNumberOfRecipesByAuthorAndIngredients(string authorName, string selectedIngredients)
        {
            var ingredients = selectedIngredients.Split(',');
            var recipesNumber = await _recipeRepository.GetNumberOfRecipesByAuthorAndIngredients(authorName, ingredients);
            return recipesNumber > 0 ? recipesNumber - 1 : 0;
        }

        public async Task<DetailedRecipeToReturn> GetRecipeById(string id)
        {
            return await _recipeRepository.GetRecipeById(id);
        }

        public async Task<List<SimilarRecipeToReturn>> GetFiveMostSimilarRecipes(string id)
        {
            return await _recipeRepository.GetFiveMostSimilarRecipes(id);
        }
	}
}
