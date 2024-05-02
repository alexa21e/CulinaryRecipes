using CulinaryRecipes.DataAccess.Abstractions;
using CulinaryRecipes.Domain;
using Neo4j.Driver;

namespace CulinaryRecipes.DataAccess
{
    public class AuthorRepository: IAuthorRepository
    {
        private readonly INeo4JDataAccess _neo4JDataAccess;

        public AuthorRepository(INeo4JDataAccess neo4jDataAccess)
        {
            _neo4JDataAccess = neo4jDataAccess;
        }

        public async Task<List<Author>> GetMostProlificAuthors(int authorsNumber)
        {
            var query = $@"MATCH (a:Author)-[:WROTE]->(r:Recipe)
                           RETURN a.name AS Author, COUNT(r) AS RecipeCount
                           ORDER BY RecipeCount DESC
                           LIMIT {authorsNumber}";

            var records = await _neo4JDataAccess.ExecuteReadPropertiesAsync(query, null);

            var authors = records.Select(record => Author.Create(record["Author"].As<string>())).ToList();

            return authors;
        }
    }
}
