using Newtonsoft.Json;

namespace CulinaryRecipes.Domain
{
	public class Recipe
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; } = string.Empty;

		[JsonProperty("description")]
		public string Description { get; set; } = string.Empty;

		[JsonProperty("preparationTime")]
		public int PreparationTime { get; set; }

		[JsonProperty("cookingTime")]
		public int CookingTime { get; set; }

		[JsonProperty("skillLevel")]
		public string SkillLevel { get; set; } = string.Empty;
	}
}
