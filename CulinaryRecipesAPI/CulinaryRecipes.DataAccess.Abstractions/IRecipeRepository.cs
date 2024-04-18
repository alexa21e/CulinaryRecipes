using CulinaryRecipes.Domain;

namespace CulinaryRecipes.DataAccess.Abstractions
{
	public interface IRecipeRepository
	{
		/*Task<ICollection<Recipe>> GetRecipes(int skip, int pageSize);*/
		Task<List<Dictionary<string, object>>> GetRecipes(int skip, int pageSize);

	}
}
