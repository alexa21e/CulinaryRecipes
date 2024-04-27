namespace CulinaryRecipes.Domain.Specifications
{
    public class IngredientParameters
    {
        public string Name { get; set; } = string.Empty;
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
