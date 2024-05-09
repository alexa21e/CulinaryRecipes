﻿using CulinaryRecipes.ApplicationServices.Abstractions;
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

        public async Task<List<HomeRecipeToReturn>> GetRecipesByAuthor(AuthorRecipeParameters param)
        {
            var skip = (param.PageNumber - 1) * param.PageSize;

            string[] ingredients = [];
            if (!string.IsNullOrEmpty(param.SelectedIngredients))
            {
                ingredients = param.SelectedIngredients.Split(',');
            }

            var recipes = await _recipeRepository.GetRecipesByAuthor(skip, param.PageSize, param.SortOrder, 
                param.AuthorName, param.RecipeName, ingredients);

            if (param.ClickedRecipe)
            {
                recipes.RemoveAll(r => r.Id == param.ClickedRecipeId);
            }
            
            return recipes;
        }

        public async Task<int> GetRecipesByAuthorCount(AuthorRecipeParameters param)
        {
            string[] ingredients = [];
            if (!string.IsNullOrEmpty(param.SelectedIngredients))
            {
                ingredients = param.SelectedIngredients.Split(',');
            }

            var recipesNumber = await _recipeRepository.GetRecipesByAuthorCount(param.AuthorName, param.RecipeName, ingredients);
           
            if (param.ClickedRecipe)
            {
                recipesNumber--;
            }

            return recipesNumber;
        }

        public async Task<List<RecipeStatsToReturn>> GetMostComplexRecipes(int recipesNumber)
        {
            return await _recipeRepository.GetMostComplexRecipes(recipesNumber);
        }

        public async Task<DetailedRecipeToReturn> GetRecipeById(string id)
        {
            return await _recipeRepository.GetRecipeById(id);
        }

        public async Task<List<SimilarRecipeToReturn>> GetFiveMostSimilarRecipes(string id)
        {
            return await _recipeRepository.GetFiveMostSimilarRecipes(id);
        }

        public async Task<RecipeNameToReturn> GetRecipeNameById(string id)
        {
            return await _recipeRepository.GetRecipeNameById(id);
        }
	}
}
