using Microsoft.AspNetCore.Mvc;

namespace CulinaryRecipesAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class RecipesController : ControllerBase
	{
		[HttpGet]
		public IActionResult GetRecipes()
		{
			return Ok("test");
		}
	}
}
