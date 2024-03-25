using JadiPametno.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JadiPametno.Controllers
{
    public class RecipeController : Controller
    {
        private readonly MyDbContext _context;

        public RecipeController(MyDbContext context)
        {
            _context = context;
        }

        [Route("recipe/{id:int}")]

        public IActionResult Index(int id)
        {
            var neededRecipe = _context.Set<Recipe>().FirstOrDefault(r => r.RecipeId == id);

            // LINQ query to find ingredients for the needed recipe
            var ingredientsForNeededRecipe = from rh in _context.RecipeHasIngredient
                                             join i in _context.Ingredient on rh.IngredientId equals i.IngredientId
                                             where rh.RecipeId == id
                                             select i;

 
            var neededIngredients = ingredientsForNeededRecipe.ToList();

            TempData["NeededRecipe"] = neededRecipe;
            TempData["NeededIngredients"] = neededIngredients;

            return View("~/Views/Recipe/Index.cshtml");
        }
    }
}

