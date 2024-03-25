using Microsoft.EntityFrameworkCore;

namespace JadiPametno.Models
{
    [Keyless]
    public class FoodHasIngredient
    {
        public int FoodId { get; set; }
        public int IngredientId { get; set; }
    }
}
