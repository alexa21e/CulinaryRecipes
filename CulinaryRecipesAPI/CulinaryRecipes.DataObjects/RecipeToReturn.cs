using CulinaryRecipes.Domain;

namespace CulinaryRecipes.DataObjects
{
	public class RecipeToReturn
	{
		public Recipe Recipe { get; set; }
		public List<string>? Ingredients { get; set; }
	}
}
