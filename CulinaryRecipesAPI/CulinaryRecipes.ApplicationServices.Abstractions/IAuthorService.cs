using CulinaryRecipes.Domain;

namespace CulinaryRecipes.ApplicationServices.Abstractions
{
    public interface IAuthorService
    {
        Task<List<Author>> GetMostProlificAuthors(int authorsNumber);
    }
}
