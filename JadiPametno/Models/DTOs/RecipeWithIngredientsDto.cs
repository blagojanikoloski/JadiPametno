namespace JadiPametno.Models.DTOs
{
    public class RecipeWithIngredientsDto
    {
        public Recipe Recipe { get; set; }
        public List<Ingredient> Ingredients { get; set; }
    }
}
