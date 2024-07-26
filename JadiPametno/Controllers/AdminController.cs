using JadiPametno.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace JadiPametno.Controllers
{
    public class AdminController : Controller
    {

        private readonly MyDbContext _context;

        public AdminController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("admin/ApproveRecipe")]
        public IActionResult Approve()
        {
            var unapprovedRecipes = _context.Recipe
                .Where(r => r.Status == 0)
                .Select(r => new RecipeWithIngredientsDto
                {
                    Recipe = r,
                    Ingredients = _context.RecipeHasIngredient
                        .Where(rh => rh.RecipeId == r.RecipeId)
                        .Join(_context.Ingredient,
                              rh => rh.IngredientId,
                              i => i.IngredientId,
                              (rh, i) => i)
                        .ToList()
                })
                .ToList();

            TempData["UnapprovedRecipes"] = unapprovedRecipes;
            return View("~/Views/Admin/ApproveRecipe.cshtml");
        }



        [HttpPost]
        [Route("admin/approveRecipe")]
        public IActionResult ApproveRecipe(int recipeId)
        {
            var recipe = _context.Recipe.Find(recipeId);
            if (recipe != null)
            {
                recipe.Status = 1; // Assuming 1 means approved
                _context.SaveChanges();
            }
            return RedirectToAction("Approve");
        }

        [HttpPost]
        [Route("admin/rejectRecipe")]
        public IActionResult RejectRecipe(int recipeId)
        {
            // Find the recipe with the given ID
            var recipe = _context.Recipe.Find(recipeId);

            // Check if the recipe exists
            if (recipe != null)
            {
                // Remove the recipe from the database
                _context.Recipe.Remove(recipe);

                // Save changes to the database
                _context.SaveChanges();
            }

            // Redirect to the "Approve" action or another relevant action/view
            return RedirectToAction("Approve");
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
