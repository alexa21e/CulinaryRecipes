using Newtonsoft.Json;

namespace CulinaryRecipes.Domain
{
	public class Ingredient
	{
		[JsonProperty("name")]
		public string Name { get; set; }
	}
}
