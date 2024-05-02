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

        public async Task<List<RecipeAsNameToReturn>> GetMostComplexRecipes(int recipesNumber)
        {
            var query = $@"MATCH (r:Recipe)-[:CONTAINS_INGREDIENT]->(i:Ingredient)
                           WITH r, COUNT(i) AS IngredientCount
                           RETURN r.id AS Id, r.name AS Name, IngredientCount, r.skillLevel, r.preparationTime, r.cookingTime, 
                             (IngredientCount + CASE r.skillLevel WHEN 'Easy' THEN 1 WHEN 'More effort' THEN 2 WHEN 'A challenge' THEN 3 END 
                             + r.preparationTime/60 + r.cookingTime/60) AS ComplexityScore
                           ORDER BY ComplexityScore DESC
                           LIMIT {recipesNumber}";

            var records = await _neo4JDataAccess.ExecuteReadPropertiesAsync(query, null);

            var recipes = records.Select(record => new RecipeAsNameToReturn(){Name = record["Name"].As<string>()}).ToList();

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
            var query = @"MATCH (r:Recipe {id: $id}) - [:CONTAINS_INGREDIENT]->(i: Ingredient)  
                          OPTIONAL MATCH (r) - [:COLLECTION]->(c: Collection)
                          OPTIONAL MATCH (r) - [:KEYWORD]->(k: Keyword)
                          OPTIONAL MATCH (r) - [:DIET_TYPE]->(d: DietType)
                          RETURN r.id AS Id, r.name AS Name, r.description AS Description, r.preparationTime AS PreparationTime, 
                                        r.cookingTime AS CookingTime, r.skillLevel AS SkillLevel, collect(DISTINCT i) AS Ingredients,
                                        collect(DISTINCT c) AS Collections, collect(DISTINCT k) AS Keywords, collect(DISTINCT d) AS DietTypes";

            var parameters = new Dictionary<string, object>
            {
                { "id", id }
            };

            var record = await _neo4JDataAccess.ExecuteReadSingleRecordAsync(query, parameters);

            if (!record.Any())
            {
                return null;
            }

            var ingredientsAsNodes = record["Ingredients"].As<ICollection<INode>>();
            var collectionsAsNodes = record["Collections"].As<ICollection<INode>>();
            var keywordsAsNodes = record["Keywords"].As<ICollection<INode>>();
            var dietAsNode = record["DietTypes"].As<ICollection<INode>>();

            var ingredients = ingredientsAsNodes.Select(node => node.Properties["name"].ToString()).ToList();
            var collections = collectionsAsNodes.Select(node => node.Properties["name"].ToString()).ToList();
            var keywords = keywordsAsNodes.Select(node => node.Properties["name"].ToString()).ToList();
            var diets = dietAsNode.Select(node => node.Properties["name"].ToString()).ToList(); ;

            var recipe = new RecipeToReturn()
            {
                Recipe = Recipe.Create(
                    (string)record["Id"],
                    (string)record["Name"],
                    (string)record["Description"],
                    (long)record["PreparationTime"],
                    (long)record["CookingTime"],
                    (string)record["SkillLevel"]),
                Ingredients = ingredients,
                Collections = collections,
                Keywords = keywords,
                DietTypes = diets
            };

            return recipe;
        }

        public async Task<List<SimilarRecipeToReturn>> GetFiveMostSimilarRecipes(string id)
        {
            var query = @"MATCH (r1:Recipe {id: $id})-[:CONTAINS_INGREDIENT|:KEYWORD|:DIET_TYPE|:COLLECTION]->(property)
                          MATCH (r2:Recipe)-[:CONTAINS_INGREDIENT|:KEYWORD|:DIET_TYPE|:COLLECTION]->(property)
                          WHERE r1 <> r2
                          WITH r1, r2, COUNT(DISTINCT property) AS intersection
                          WHERE intersection > 0
                          MATCH (r1)-[:CONTAINS_INGREDIENT|:KEYWORD|:DIET_TYPE|:COLLECTION]->(p1)
                          WITH r1, r2, intersection, COUNT(DISTINCT p1) AS totalPropertiesR1
                          MATCH (r2)-[:CONTAINS_INGREDIENT|:KEYWORD|:DIET_TYPE|:COLLECTION]->(p2)
                          WITH r1, r2, intersection, totalPropertiesR1, COUNT(DISTINCT p2) AS totalPropertiesR2
                          WITH r1, r2, intersection, totalPropertiesR1 + totalPropertiesR2 - intersection AS union
                          RETURN r2.id AS Id, r2.name AS Name, intersection, union
                          ORDER BY (toFloat(intersection) / toFloat(union)) DESC
                          LIMIT 5";

            var parameters = new Dictionary<string, object>
            {
                { "id", id }
            };

            var records = await _neo4JDataAccess.ExecuteReadPropertiesAsync(query, parameters);

            var similarRecipes = records.Select(record => new SimilarRecipeToReturn
            {
                Id = (string)record["Id"],
                Name = (string)record["Name"],
                Similarity = ((long)record["intersection"] / (double)(long)record["union"]) * 100
            }).ToList();

            return similarRecipes;
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
