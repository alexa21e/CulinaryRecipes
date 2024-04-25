using System.Collections;
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

		public async Task<List<RecipesToReturn>> GetRecipes(int skip, int pageSize)
		{
			const string query = @"MATCH(r: Recipe) - [:CONTAINS_INGREDIENT]->(i: Ingredient), (a: Author) - [:WROTE]->(r)
				                   RETURN r.id AS Id, r.name AS Name, a.name AS Author, count(i) AS NumberOfIngredients, r.skillLevel AS SkillLevel
				                   ORDER BY r.name      
                                   SKIP $skip 
                                   LIMIT $pageSize";

			var parameters = new Dictionary<string, object>
			{
				{ "skip", skip },
				{ "pageSize", pageSize }
			};

			var records = await _neo4JDataAccess.ExecuteReadPropertiesAsync(query, parameters);

            var recipes = records.Select(record => new RecipesToReturn
            {
                Id = record["Id"].As<string>(),
                Name = record["Name"].As<string>(),
                Author = record["Author"].As<string>(),
                NumberOfIngredients = record["NumberOfIngredients"].As<int>(),
                SkillLevel = record["SkillLevel"].As<string>()
            }).ToList();

            return recipes;
		}

        public async Task<List<RecipesToReturn>> GetRecipesByName(string name, int skip, int pageSize)
        {
            const string query = @"MATCH (r:Recipe)-[:CONTAINS_INGREDIENT]->(i:Ingredient), (a:Author)-[:WROTE]->(r)
                                   WHERE r.name CONTAINS $name
                                   RETURN r.id AS Id, r.name AS Name, a.name AS Author, count(i) AS NumberOfIngredients, r.skillLevel AS SkillLevel
                                   ORDER BY r.name      
                                   SKIP $skip 
                                   LIMIT $pageSize";

            var parameters = new Dictionary<string, object>
            {
                { "name", name },
                { "skip", skip },
                { "pageSize", pageSize }
            };

            var records = await _neo4JDataAccess.ExecuteReadPropertiesAsync(query, parameters);

            var recipes = records.Select(record => new RecipesToReturn
            {
                Id = record["Id"].As<string>(),
                Name = record["Name"].As<string>(),
                Author = record["Author"].As<string>(),
                NumberOfIngredients = record["NumberOfIngredients"].As<int>(),
                SkillLevel = record["SkillLevel"].As<string>()
            }).ToList();

            return recipes;
        }

        public async Task<int> GetNumberOfRecipes()
		{
			const string query = @"MATCH (r:Recipe) RETURN count(r) AS NumberOfRecipes";
			var numberOfRecipes = await _neo4JDataAccess.ExecuteReadScalarAsync<int>(query);

			return numberOfRecipes;
		}

        public async Task<int> GetNumberOfRecipesByName(string name)
        {
            const string query = @"MATCH (r:Recipe) WHERE r.name CONTAINS $name RETURN count(r) AS NumberOfRecipes";

            var parameters = new Dictionary<string, object>
            {
                { "name", name }
            };

            var numberOfRecipes = await _neo4JDataAccess.ExecuteReadScalarAsync<int>(query, parameters);

            return numberOfRecipes;
        }

        public async Task<RecipeToReturn> GetRecipeById(string id)
        {
            const string query = @"MATCH (r:Recipe {id: $id}) - [:CONTAINS_INGREDIENT]->(i: Ingredient)  
                                 RETURN r.id AS Id, r.name AS Name, r.description AS Description, r.preparationTime AS PreparationTime, 
                                        r.cookingTime AS CookingTime, r.skillLevel AS SkillLevel, collect(i) AS Ingredients";

            var parameters = new Dictionary<string, object>
            {
                { "id", id }
            };

            var record = await _neo4JDataAccess.ExecuteReadPropertiesAsync(query, parameters);

            if (record == null || !record.Any())
            {
                return null;
            }

            var ingredientsAsNodes = record[0]["Ingredients"].As<ICollection<INode>>();

            var ingredients = ingredientsAsNodes.Select(node => node.Properties["name"].ToString()).ToList();

            var recipe = new RecipeToReturn()
            {
                Recipe = Recipe.Create(
                    (string)record[0]["Id"],
                    (string)record[0]["Name"],
                    (string)record[0]["Description"],
                    (long)record[0]["PreparationTime"],
                    (long)record[0]["CookingTime"],
                    (string)record[0]["SkillLevel"]),
                Ingredients = ingredients
            };

            return recipe;
        }
    }
}
