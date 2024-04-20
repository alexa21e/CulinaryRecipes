using CulinaryRecipes.DataAccess.Abstractions;

namespace CulinaryRecipes.DataAccess
{
	public class RecipeRepository: IRecipeRepository
	{
		private readonly INeo4JDataAccess _neo4JDataAccess;

		public RecipeRepository(INeo4JDataAccess neo4jDataAccess)
		{
			_neo4JDataAccess = neo4jDataAccess;
		}

		public async Task<List<Dictionary<string, object>>> GetRecipes(int skip, int pageSize)
		{
			var query = @"MATCH (r:Recipe)-[:CONTAINS]->(i:Ingredient), (r)-[:CREATED_BY]->(a:Author)
                  RETURN r.Name AS Name, a.Name AS Author, count(i) AS NumberOfIngredients, r.SkillLevel AS SkillLevel 
                  ORDER BY r.Name SKIP $skip LIMIT $pageSize";
			//var query = @"MATCH(r: Recipe) - [:CONTAINS_INGREDIENT]->(i: Ingredient), (a: Author) - [:WROTE]->(r)
			//	RETURN r.name AS Name, a.name AS Author, count(i) AS NumberOfIngredients, r.skillLevel AS SkillLevel
			//	ORDER BY r.name SKIP $skip LIMIT $pageSize";

			var parameters = new Dictionary<string, object>
			{
				{ "skip", skip },
				{ "pageSize", pageSize }
			};

			var recipes = await _neo4JDataAccess.ExecuteReadPropertiesAsync(query, parameters);

			return recipes;
		}

		public async Task<int> GetNumberOfRecipes()
		{
			var query = @"MATCH (r:Recipe) RETURN count(r) AS NumberOfRecipes";
			//var query = @"MATCH (r:Recipe) RETURN count(r) AS numberOfRecipes";
			var numberOfRecipes = await _neo4JDataAccess.ExecuteReadScalarAsync<int>(query);

			return numberOfRecipes;
		}
	}
}
