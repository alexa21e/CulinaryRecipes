using CulinaryRecipes.ApplicationServices.Abstractions;
using CulinaryRecipes.DataAccess.Abstractions;
using CulinaryRecipes.Domain;

namespace CulinaryRecipes.ApplicationServices
{
    public class AuthorService: IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorService(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<List<Author>> GetMostProlificAuthors(int authorsNumber)
        {
            return await _authorRepository.GetMostProlificAuthors(authorsNumber);
        }
    }
}
