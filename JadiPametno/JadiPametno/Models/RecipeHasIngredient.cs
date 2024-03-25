using Microsoft.EntityFrameworkCore;

namespace JadiPametno.Models
{
    [Keyless]
    public class RecipeHasIngredient
    {
        public int RecipeId { get; set; }
        public int IngredientId { get; set; }
    }
}
