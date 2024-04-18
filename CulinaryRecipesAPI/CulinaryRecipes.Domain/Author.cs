using Newtonsoft.Json;

namespace CulinaryRecipes.Domain
{
	public class Author
	{
		
		public string Name { get; set; }

		private Author(){}

		public static Author Create(string name)
		{
			return new Author
			{
				Name = name
			};
		}
	}
}
