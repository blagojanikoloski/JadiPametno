using Microsoft.EntityFrameworkCore;

namespace JadiPametno.Models
{
    public class RecipeHasIngredient
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public int IngredientId { get; set; }
    }
}
