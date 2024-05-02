using CulinaryRecipes.ApplicationServices.Abstractions;
using CulinaryRecipes.Domain;
using Microsoft.AspNetCore.Mvc;

namespace CulinaryRecipesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet("mostprolific")]
        public async Task<ActionResult<List<Author>>> GetMostProlificAuthors([FromQuery] int authorsNumber)
        {
            return await _authorService.GetMostProlificAuthors(authorsNumber);
        }
    }
}
