﻿using CulinaryRecipes.Domain.Specifications;

namespace CulinaryRecipes.ApplicationServices.Abstractions
{
	public interface IRecipeService
	{
		Task<List<Dictionary<string, object>>> GetRecipes(RecipeParameters param);
		Task<int> GetNumberOfRecipes();
	}
}
