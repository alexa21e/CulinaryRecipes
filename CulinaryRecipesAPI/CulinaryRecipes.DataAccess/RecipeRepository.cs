using CulinaryRecipes.DataAccess.Abstractions;
using CulinaryRecipes.DataObjects;
using CulinaryRecipes.Domain;
using Neo4j.Driver;

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
			var query = @"MATCH(r: Recipe) - [:CONTAINS_INGREDIENT]->(i: Ingredient), (a: Author) - [:WROTE]->(r)
				RETURN r.id AS Id, r.name AS Name, a.name AS Author, count(i) AS NumberOfIngredients, r.skillLevel AS SkillLevel
				ORDER BY r.name SKIP $skip LIMIT $pageSize";

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
			var numberOfRecipes = await _neo4JDataAccess.ExecuteReadScalarAsync<int>(query);

			return numberOfRecipes;
		}

        public async Task<Recipe> GetRecipeById(string id)
        {
            const string query = @"MATCH (r:Recipe {id: $id})
                           RETURN r.id AS Id, r.name AS Name, r.description AS Description, 
                                  r.preparationTime AS PreparationTime, r.cookingTime AS CookingTime, 
                                  r.skillLevel AS SkillLevel";

            var parameters = new Dictionary<string, object>
            {
                { "id", id }
            };

            var record = await _neo4JDataAccess.ExecuteReadPropertiesAsync(query, parameters);

            if (record == null || !record.Any())
            {
                return null;
            }

            var firstRecord = record.First();

            var recipe = Recipe.Create(
                (string)firstRecord["Id"],
                    (string)firstRecord["Name"],
                    (string)firstRecord["Description"],
                    (int)(long)firstRecord["PreparationTime"],
                    (int)(long)firstRecord["CookingTime"],
                    (string)firstRecord["SkillLevel"]);

            return recipe;
        }

        //public async Task<RecipeToReturn> GetRecipeById(int id)
        //{
        //    const string query = @"MATCH (r:Recipe {id: $id}) - [:CONTAINS_INGREDIENT]->(i: Ingredient)  
        //                 RETURN r.id AS Id, r.name AS Name, r.description AS Description, r.preparationTime AS PreparationTime, 
        //                        r.cookingTime AS CookingTime, r.skillLevel AS SkillLevel, collect(i) AS Ingredients";

        //    var parameters = new Dictionary<string, object>
        //    {
        //        { "id", id }
        //    };

        //    var record = await _neo4JDataAccess.ExecuteReadPropertiesWithNodesAsync(query, parameters);

        //    if (record == null || !record.Any())
        //    {
        //        return null;
        //    }

        //    var firstRecord = record.First();

        //    var recipe = new RecipeToReturn()
        //    {
        //        Recipe = Recipe.Create(
        //            (string)firstRecord["Id"],
        //            (string)firstRecord["Name"],
        //            (string)firstRecord["Description"],
        //            (int)firstRecord["PreparationTime"],
        //            (int)firstRecord["CookingTime"],
        //            (string)firstRecord["SkillLevel"]),
        //        Ingredients = ((List<Dictionary<string, object>>)firstRecord["Ingredients"]).Select(ingredientRecord =>
        //            Ingredient.Create((string)ingredientRecord["Name"])).ToList()
        //    };

        //    return recipe;
        //}
    }
}
