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

		public async Task<List<RecipesToReturn>> GetRecipes(RecipeParameters param)
		{
			var skip = (param.PageNumber - 1) * param.PageSize;
			return await _recipeRepository.GetRecipes(skip, param.PageSize);
		}

        public async Task<List<RecipesToReturn>> GetRecipesByName(string name, RecipeParameters param)
        {
            var skip = (param.PageNumber - 1) * param.PageSize;
            return await _recipeRepository.GetRecipesByName(name, skip, param.PageSize);
        }

		public async Task<List<RecipesToReturn>> GetRecipesByIngredients(string[] selectedIngredients, RecipeParameters param) {
            var skip = (param.PageNumber - 1) * param.PageSize;
            return await _recipeRepository.GetRecipesByIngredients(selectedIngredients, skip, param.PageSize);
        }

        public async Task<List<RecipesToReturn>> GetRecipesByAuthor(string authorName, string clickedRecipeId,
            RecipeParameters param)
        {
            var skip = (param.PageNumber - 1) * param.PageSize;
            var recipes = await _recipeRepository.GetRecipesByAuthor(authorName, skip, param.PageSize);
            recipes.RemoveAll(r => r.Id == clickedRecipeId);
            return recipes;
        }

		public async Task<int> GetNumberOfRecipes()
		{
			return await _recipeRepository.GetNumberOfRecipes();
		}

        public async Task<int> GetNumberOfRecipesByName(string name)
        {
            return await _recipeRepository.GetNumberOfRecipesByName(name);
        }

        public async Task<int> GetNumberOfRecipesByIngredients(string[] selectedIngredients)
        {
            return await _recipeRepository.GetNumberOfRecipesByIngredients(selectedIngredients);
        }

        public async Task<int> GetNumberOfRecipesByAuthor(string authorName){
            var recipes = await _recipeRepository.GetNumberOfRecipesByAuthor(authorName);
            return recipes - 1;
        }

        public async Task<RecipeToReturn> GetRecipeById(string id)
        {
            return await _recipeRepository.GetRecipeById(id);
        }
	}
}
