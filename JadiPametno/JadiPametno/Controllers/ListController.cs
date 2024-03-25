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
            List<Recipe> selectedRecipes = new List<Recipe>();
            int totalCalories = 0;
            int remainingMeals = numberOfMeals;

            // Shuffle recipes to have a random selection each time
            recipes.Shuffle();

            foreach (var recipe in recipes)
            {
                if (remainingMeals <= 0 || totalCalories >= targetCalories)
                {
                    // If we have enough meals or calories, break the loop
                    break;
                }

                int newTotalCalories = totalCalories + (int)recipe.Calories;

                // Check if adding this recipe will keep the total calories within the desired range
                if (Math.Abs(newTotalCalories - targetCalories) <= 150 && remainingMeals == 1)
                {
                    // If this recipe makes the total calories within range and it's the last meal needed, add it
                    selectedRecipes.Add(recipe);
                    totalCalories = newTotalCalories;
                    remainingMeals--;
                }
                else if (newTotalCalories <= targetCalories && remainingMeals > 1)
                {
                    // If adding this recipe keeps us below the target calories and more meals are needed, add it
                    selectedRecipes.Add(recipe);
                    totalCalories = newTotalCalories;
                    remainingMeals--;
                }
            }

            // Check if the total calories are still below the target after selecting the maximum number of meals
            if (totalCalories < targetCalories && remainingMeals == 0)
            {
                // Iterate through recipes again to fill the remaining calories
                foreach (var recipe in recipes)
                {
                    int newTotalCalories = totalCalories + (int)recipe.Calories;

                    if (newTotalCalories <= targetCalories)
                    {
                        // If adding this recipe keeps us below or at the target calories, add it
                        selectedRecipes.Add(recipe);
                        totalCalories = newTotalCalories;

                        if (totalCalories >= targetCalories)
                        {
                            // If we've reached the target calories, break the loop
                            break;
                        }
                    }
                }
            }


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
