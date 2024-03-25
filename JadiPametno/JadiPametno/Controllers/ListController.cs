using JadiPametno.Models; // Import the necessary models
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq; // Import System.Linq for querying
using Microsoft.EntityFrameworkCore; // Import Entity Framework Core

namespace JadiPametno.Controllers
{
    public class ListController : Controller
    {
        private readonly MyDbContext _context; // Use DbContext directly

        // Inject DbContext through constructor
        public ListController(MyDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Fetch data from the database using Entity Framework Core
            var foods = _context.Set<Food>().ToList();
            var ingredients = _context.Set<Ingredient>().ToList(); 
            var recipes = _context.Set<Recipe>().ToList(); 

            // Pass the fetched data to the view
            ViewBag.Foods = foods;
            ViewBag.Ingredients = ingredients;
            ViewBag.Recipes = recipes;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
