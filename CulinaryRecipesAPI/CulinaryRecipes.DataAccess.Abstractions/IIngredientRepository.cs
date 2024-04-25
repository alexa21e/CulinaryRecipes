using CulinaryRecipes.Domain;

namespace CulinaryRecipes.DataAccess.Abstractions
{
    public interface IIngredientRepository
    {
        Task<List<Ingredient>> GetIngredients(string name, int skip, int pageSize);
        Task<int> GetNumberOfIngredients(string name);
    }
}
