namespace CulinaryRecipesAPI.Helpers
{
    public class Listing<T> where T : class
    {
        public int IngredientsDisplayedNo { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }
        public Listing(int ingredientsDisplayedNo, int count, IReadOnlyList<T> data)
        {
            IngredientsDisplayedNo = ingredientsDisplayedNo;
            Count = count;
            Data = data;
        }
    }
}
