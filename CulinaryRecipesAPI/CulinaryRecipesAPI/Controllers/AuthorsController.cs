using CulinaryRecipes.ApplicationServices.Abstractions;
using CulinaryRecipes.Domain;
using Microsoft.AspNetCore.Mvc;

namespace CulinaryRecipesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet("mostprolific")]
        public async Task<ActionResult<List<Author>>> GetMostProlificAuthors(int authorsNumber)
        {
            return await _authorService.GetMostProlificAuthors(authorsNumber);
        }
    }
}
