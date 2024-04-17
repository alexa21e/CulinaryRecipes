using Newtonsoft.Json;

namespace CulinaryRecipes.Domain
{
	public class Author
	{
		[JsonProperty("name")]
		public string Name { get; set; }
	}
}
