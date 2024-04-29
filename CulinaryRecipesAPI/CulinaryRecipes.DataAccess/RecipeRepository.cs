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

		public async Task<List<RecipesToReturn>> GetRecipes(int skip, int pageSize, string sortOrder)
		{
            var order = ParseSortOrder(sortOrder);

			var query= $@"MATCH(r: Recipe) - [:CONTAINS_INGREDIENT]->(i: Ingredient), (a: Author) - [:WROTE]->(r)
				          RETURN r.id AS Id, r.name AS Name, a.name AS Author, count(i) AS NumberOfIngredients, r.skillLevel AS SkillLevel
				          {order}     
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

        public async Task<List<RecipesToReturn>> GetRecipesByName(string name, int skip, int pageSize, string sortOrder)
        {
            var order = ParseSortOrder(sortOrder);

            var query = $@"MATCH (r:Recipe)-[:CONTAINS_INGREDIENT]->(i:Ingredient), (a:Author)-[:WROTE]->(r)
                           WHERE r.name CONTAINS $name
                           RETURN r.id AS Id, r.name AS Name, a.name AS Author, count(i) AS NumberOfIngredients, r.skillLevel AS SkillLevel
                           {order}      
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

        public async Task<List<RecipesToReturn>> GetRecipesByIngredients(string[] selectedIngredients, int skip, int pageSize, string sortOrder)
        {
            var order = ParseSortOrder(sortOrder);

            var matchClauses = selectedIngredients.Select((ingredient, index) => 
                                                $"MATCH (r)-[:CONTAINS_INGREDIENT]->(:Ingredient {{name: $ingredient{index}}})").ToList();

            var query = $@"MATCH (r:Recipe)-[:CONTAINS_INGREDIENT]->(i:Ingredient), (a:Author)-[:WROTE]->(r) " + string.Join(" ", matchClauses) + 
                              $" RETURN r.id AS Id, r.name AS Name, a.name AS Author, count(i) AS NumberOfIngredients, r.skillLevel AS SkillLevel {order} SKIP $skip LIMIT $pageSize";

            var parameters = new Dictionary<string, object>
            {
                { "skip", skip },
                { "pageSize", pageSize }
            };

            for (int i = 0; i < selectedIngredients.Count(); i++)
            {
                parameters.Add($"ingredient{i}", selectedIngredients[i]);
            }

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

        public async Task<List<RecipesToReturn>> GetRecipesByAuthor(string authorName, int skip, int pageSize, string sortOrder)
        {
            var order = ParseSortOrder(sortOrder);

            var query = $@"MATCH (r:Recipe)-[:CONTAINS_INGREDIENT]->(i:Ingredient), (a:Author)-[:WROTE]->(r)
                           WHERE a.name = $authorName
                           RETURN r.id AS Id, r.name AS Name, a.name AS Author, count(i) AS NumberOfIngredients, r.skillLevel AS SkillLevel
                           {order}
                           SKIP $skip
                           LIMIT $pageSize";

            var parameters = new Dictionary<string, object>
            {
                { "authorName", authorName },
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

        public async Task<List<RecipesToReturn>> GetRecipesByAuthorAndName(string authorName, string recipeName, int skip, int pageSize,
            string sortOrder)
        {
            var order = ParseSortOrder(sortOrder);

            var query = $@"MATCH (r:Recipe)-[:CONTAINS_INGREDIENT]->(i:Ingredient), (a:Author)-[:WROTE]->(r)
                           WHERE a.name = $authorName AND r.name CONTAINS $recipeName
                           RETURN r.id AS Id, r.name AS Name, a.name AS Author, count(i) AS NumberOfIngredients, r.skillLevel AS SkillLevel
                           {order}
                           SKIP $skip
                           LIMIT $pageSize";

            var parameters = new Dictionary<string, object>
            {
                { "authorName", authorName },
                { "recipeName", recipeName },
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

        public async Task<List<RecipesToReturn>> GetRecipesByAuthorAndIngredients(string authorName,
            string[] selectedIngredients, int skip, int pageSize, string sortOrder)
        {
            var order = ParseSortOrder(sortOrder);

            var matchClauses = selectedIngredients.Select((ingredient, index) =>
                                                               $"MATCH (r)-[:CONTAINS_INGREDIENT]->(:Ingredient {{name: $ingredient{index}}})").ToList();

            var query = $@"MATCH (r:Recipe)-[:CONTAINS_INGREDIENT]->(i:Ingredient), (a:Author)-[:WROTE]->(r) " + string.Join(" ", matchClauses) +
                              " WHERE a.name = $authorName" +
                              " RETURN r.id AS Id, r.name AS Name, a.name AS Author, count(i) AS NumberOfIngredients, r.skillLevel AS SkillLevel" +
                              " {order} SKIP $skip LIMIT $pageSize";

            var parameters = new Dictionary<string, object>
            {
                { "authorName", authorName },
                { "skip", skip },
                { "pageSize", pageSize }
            };

            for (int i = 0; i < selectedIngredients.Count(); i++)
            {
                parameters.Add($"ingredient{i}", selectedIngredients[i]);
            }

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

        public async Task<int> GetNumberOfRecipesByIngredients(string[] selectedIngredients)
        {
            var matchClauses = selectedIngredients.Select((ingredient, index) =>
                                                               $"MATCH (r)-[:CONTAINS_INGREDIENT]->(:Ingredient {{name: $ingredient{index}}})").ToList();

            var query = @"MATCH (r:Recipe)" + string.Join(" ", matchClauses) +
                              " RETURN count(r) AS NumberOfRecipes";

            var parameters = new Dictionary<string, object>();

            for (int i = 0; i < selectedIngredients.Count(); i++)
            {
                parameters.Add($"ingredient{i}", selectedIngredients[i]);
            }

            var numberOfRecipes = await _neo4JDataAccess.ExecuteReadScalarAsync<int>(query, parameters);

            return numberOfRecipes;
        }

        public async Task<int> GetNumberOfRecipesByAuthor(string authorName)
        {
            const string query = @"MATCH (r:Recipe), (a:Author)-[:WROTE]->(r)
                                   WHERE a.name = $authorName 
                                   RETURN count(r) AS NumberOfRecipes";

            var parameters = new Dictionary<string, object>
            {
                { "authorName", authorName }
            };

            var numberOfRecipes = await _neo4JDataAccess.ExecuteReadScalarAsync<int>(query, parameters);

            return numberOfRecipes;
        }

        public async Task<int> GetNumberOfRecipesByAuthorAndName(string authorName, string recipeName)
        {
            const string query = @"MATCH (r:Recipe), (a:Author)-[:WROTE]->(r)
                                   WHERE a.name = $authorName AND r.name CONTAINS $recipeName
                                   RETURN count(r) AS NumberOfRecipes";

            var parameters = new Dictionary<string, object>
            {
                { "authorName", authorName },
                { "recipeName", recipeName }
            };

            var numberOfRecipes = await _neo4JDataAccess.ExecuteReadScalarAsync<int>(query, parameters);

            return numberOfRecipes;
        }

        public async Task<int> GetNumberOfRecipesByAuthorAndIngredients(string authorName, string[] selectedIngredients)
        {
            var matchClauses = selectedIngredients.Select((ingredient, index) =>
                                                                              $"MATCH (r)-[:CONTAINS_INGREDIENT]->(:Ingredient {{name: $ingredient{index}}})").ToList();

            var query = @"MATCH (r:Recipe), (a:Author)-[:WROTE]->(r) " + string.Join(" ", matchClauses) +
                              " WHERE a.name = $authorName" +
                              " RETURN count(r) AS NumberOfRecipes";

            var parameters = new Dictionary<string, object>
            {
                { "authorName", authorName }
            };

            for (int i = 0; i < selectedIngredients.Count(); i++)
            {
                parameters.Add($"ingredient{i}", selectedIngredients[i]);
            }

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

        private string ParseSortOrder(string sortOrder)
        {
            var parts = sortOrder.Split('_');
            var column = parts[0];
            var direction = parts[1].ToUpper();

            return column switch
            {
                "numberOfIngredients" => $"ORDER BY count(i) {direction}, r.name ASC",
                "skillLevel" => $"ORDER BY CASE r.skillLevel WHEN 'Easy' THEN 1 WHEN 'More effort' THEN 2 WHEN 'A challenge' THEN 3 END {direction}, r.name ASC",
                _ => $"ORDER BY r.name {direction}",
            };
        }
    }
}
