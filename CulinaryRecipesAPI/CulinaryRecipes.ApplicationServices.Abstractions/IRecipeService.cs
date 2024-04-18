using CulinaryRecipes.Domain;

namespace CulinaryRecipes.ApplicationServices.Abstractions
{
	public interface IRecipeService
	{
		Task<List<Dictionary<string, object>>> GetRecipes(int pageNumber, int pageSize);
	}
}
