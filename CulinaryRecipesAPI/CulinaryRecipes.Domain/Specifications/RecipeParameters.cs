﻿namespace CulinaryRecipes.Domain.Specifications
{
	public class RecipeParameters
	{
		public int PageNumber { get; set; } = 1;
		public int PageSize { get; set; } = 20;
	}
}
