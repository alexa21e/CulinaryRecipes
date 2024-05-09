﻿using CulinaryRecipes.DataAccess.Abstractions;
using CulinaryRecipes.Domain;
using Neo4j.Driver;

namespace CulinaryRecipes.DataAccess
{
    public class IngredientRepository: IIngredientRepository
    {
        private readonly INeo4JDataAccess _neo4JDataAccess;

        public IngredientRepository(INeo4JDataAccess neo4jDataAccess)
        {
            _neo4JDataAccess = neo4jDataAccess;
        }

        public async Task<List<Ingredient>> GetIngredients(string name, int ingredientsDisplayedNo)
        {
            var query = @"MATCH (i:Ingredient) WHERE toLower(i.name) CONTAINS $name 
                                 RETURN i.name AS Name
                                 ORDER BY i.name 
                                 LIMIT $ingredientsDisplayedNo";

            var parameters = new Dictionary<string, object>
            {
                { "name", name },
                { "ingredientsDisplayedNo", ingredientsDisplayedNo }
            };

            var records = await _neo4JDataAccess.ExecuteReadPropertiesAsync(query, parameters);

            var ingredients = records.Select(record => Ingredient.Create(record["Name"].As<string>())).ToList();

            return ingredients;
        }

        public async Task<List<Ingredient>> GetMostCommonIngredients(int ingredientsNumber)
        {
            var query = $@"MATCH (r:Recipe)-[:CONTAINS_INGREDIENT]->(i:Ingredient) 
                          RETURN i.name AS Ingredient, COUNT(r) AS RecipeCount 
                          ORDER BY RecipeCount DESC 
                          LIMIT {ingredientsNumber}";

            var records = await _neo4JDataAccess.ExecuteReadPropertiesAsync(query, null);

            var ingredients = records.Select(record => Ingredient.Create(record["Ingredient"].As<string>())).ToList();

            return ingredients;
        }

        public async Task<int> GetNumberOfIngredients(string name)
        {
            var query = @"MATCH (i:Ingredient) WHERE i.name CONTAINS $name 
                                 RETURN count(i) AS NumberOfIngredients";

            var parameters = new Dictionary<string, object>
            {
                { "name", name }
            };

            var numberOfIngredients = await _neo4JDataAccess.ExecuteReadScalarAsync<int>(query, parameters);

            return numberOfIngredients;
        }
    }
}
