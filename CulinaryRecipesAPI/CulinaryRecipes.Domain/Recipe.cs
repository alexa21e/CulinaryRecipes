using Newtonsoft.Json;

namespace CulinaryRecipes.Domain
{
	public class Recipe
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public int PreparationTime { get; set; }
		public int CookingTime { get; set; }
		public string SkillLevel { get; set; } = string.Empty;

		private Recipe() {}

		public static Recipe Create(int id, string name, string description, int preparationTime, int cookingTime,
			string skillLevel)
		{
			return new Recipe
			{
				Id = id,
				Name = name,
				Description = description,
				PreparationTime = preparationTime,
				CookingTime = cookingTime,
				SkillLevel = skillLevel
			};
		}
	}
}
