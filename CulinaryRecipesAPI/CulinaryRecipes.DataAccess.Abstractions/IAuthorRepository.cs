using CulinaryRecipes.Domain;

namespace CulinaryRecipes.DataAccess.Abstractions
{
    public interface IAuthorRepository
    {
        Task<List<Author>> GetMostProlificAuthors(int authorsNumber);
    }
}
