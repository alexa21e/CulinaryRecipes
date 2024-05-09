using CulinaryRecipes.ApplicationServices.Abstractions;
using CulinaryRecipes.DataAccess.Abstractions;
using CulinaryRecipes.Domain;
using CulinaryRecipes.Domain.Specifications;

namespace CulinaryRecipes.ApplicationServices
{
    public class IngredientService: IIngredientService
    {
        private readonly IIngredientRepository _ingredientRepository;

        public IngredientService(IIngredientRepository ingredientRepository)
        {
            _ingredientRepository = ingredientRepository;
        }

        public async Task<List<Ingredient>> GetIngredients(IngredientParameters param)
        {
            return await _ingredientRepository.GetIngredients(param.Name, param.IngredientsDisplayedNo);
        }

        public async Task<List<Ingredient>> GetMostCommonIngredients(int ingredientsNumber)
        {
            return await _ingredientRepository.GetMostCommonIngredients(ingredientsNumber);
        }

        public async Task<int> GetNumberOfIngredients(string name)
        {
            return await _ingredientRepository.GetNumberOfIngredients(name);
        }
    }
}
