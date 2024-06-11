namespace JadiPametno.Models.DTOs
{
    public class RecipeAddDto
    {
        public string RecipeName { get; set; }
        public string Calories { get; set; }
        public string RecipeType { get; set; }
        public List<int> SelectedIngredients { get; set; }
        public string Instructions { get; set; }
        public string ImageUrl { get; set; }
    }

}
