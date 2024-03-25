namespace JadiPametno.Models
{
    public class Recipe
    {
        public int RecipeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int FoodId { get; set; }
        public string Type { get; set; }
        public float Calories { get; set; }
        public string ImageUrl { get; set; }

    }
}
