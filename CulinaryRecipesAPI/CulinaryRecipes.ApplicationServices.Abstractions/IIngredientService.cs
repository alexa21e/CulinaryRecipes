using CulinaryRecipes.Domain.Specifications;
using CulinaryRecipes.Domain;

namespace CulinaryRecipes.ApplicationServices.Abstractions
{
    public interface IIngredientService
    {
        Task<List<Ingredient>> GetIngredients(IngredientParameters param);
        Task<int> GetNumberOfIngredients(string name);
    }
}
