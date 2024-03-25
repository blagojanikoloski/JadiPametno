using JadiPametno.Models;
using Microsoft.AspNetCore.Mvc;

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
            TempData["NeededRecipe"] = neededRecipe;

            return View("~/Views/Recipe/Index.cshtml");
        }
    }
}

