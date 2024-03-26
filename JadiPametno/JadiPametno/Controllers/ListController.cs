using JadiPametno.Models; // Import the necessary models
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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

        [HttpPost]
        public IActionResult Index(IFormCollection form)
        {
            // Retrieve values from the form
            int calories = int.Parse(form["calories"]);
            int numberOfMeals = int.Parse(form["numberOfMeals"]);

            // Fetch data from the database using Entity Framework Core
            var recipes = _context.Set<Recipe>().ToList(); // Retrieve all recipes

            // Select combination of recipes that are around the desired calories
            var selectedRecipes = GetRecipesForCalories(recipes, calories, numberOfMeals);

            // Pass the selected recipes and other data to the view
            TempData["Recipes"] = selectedRecipes;
            TempData["Calories"] = calories;
            TempData["NumberOfMeals"] = numberOfMeals;

            return View();
        }



        private List<Recipe> GetRecipesForCalories(List<Recipe> recipes, int targetCalories, int numberOfMeals)
        {
            // Shuffle recipes to introduce randomness
            recipes.Shuffle();

            int totalCalories = 0;
            List<Recipe> selectedRecipes = new List<Recipe>();

            // Iterate through shuffled recipes until the desired number of meals is selected
            foreach (var recipe in recipes)
            {
                if (selectedRecipes.Count == numberOfMeals || totalCalories > targetCalories + 200)
                    break;

                // Add recipe to selected recipes if adding it keeps total calories within range
                if (totalCalories + recipe.Calories <= targetCalories + 200)
                {
                    selectedRecipes.Add(recipe);
                    totalCalories += (int)recipe.Calories;
                }
            }
            // Update TempData with total calories
            TempData["TotalCalories"] = totalCalories;

            return selectedRecipes;
        }







        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    // Extension method to shuffle a list
    public static class ListExtensions
    {
        private static Random rng = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
