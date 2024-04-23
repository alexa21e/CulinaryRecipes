using CulinaryRecipes.Domain;

namespace CulinaryRecipes.DataObjects
{
	public class RecipeToReturn
	{
		public Recipe Recipe { get; set; }
		public List<Ingredient> Ingredients { get; set; }
	}
}
