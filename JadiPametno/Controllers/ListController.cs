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
            string dietType = form["dietType"];

            List<Recipe> recipes;
            if (dietType.Equals("all"))
            {
                recipes = _context.Set<Recipe>().Where(r => r.Status == 1).ToList();
            }
            else
            {
                recipes = _context.Set<Recipe>().Where(r => r.Type.Equals(dietType) && r.Status == 1).ToList();
            }

            // Select combination of recipes that are around the desired calories
            var selectedRecipes = GetRecipesForCalories(recipes, calories, numberOfMeals);

            // Pass the selected recipes and other data to the view
            TempData["Recipes"] = selectedRecipes;
            TempData["Calories"] = calories;
            TempData["NumberOfMeals"] = numberOfMeals;
            TempData["DietType"] = dietType;

            return View();
        }



        private List<List<Recipe>> GetAllRecipeCombinations(List<Recipe> recipes, int targetCalories, int numberOfMeals)
        {
            List<List<Recipe>> allCombinations = new List<List<Recipe>>();
            FindRecipeCombinations(allCombinations, new List<Recipe>(), recipes, 0, targetCalories, numberOfMeals);
            return allCombinations;
        }

        private void FindRecipeCombinations(List<List<Recipe>> allCombinations, List<Recipe> currentCombination, List<Recipe> remainingRecipes, int index, int targetCalories, int numberOfMeals)
        {
            if (currentCombination.Count == numberOfMeals)
            {
                int totalCalories = (int)currentCombination.Sum(recipe => recipe.Calories);
                if (totalCalories >= targetCalories - 100 * numberOfMeals && totalCalories <= targetCalories + 100 * numberOfMeals)
                {
                    allCombinations.Add(new List<Recipe>(currentCombination));
                }
                return;
            }

            for (int i = index; i < remainingRecipes.Count; i++)
            {
                currentCombination.Add(remainingRecipes[i]);
                FindRecipeCombinations(allCombinations, currentCombination, remainingRecipes, i + 1, targetCalories, numberOfMeals);
                currentCombination.RemoveAt(currentCombination.Count - 1);
            }
        }

        private List<Recipe> GetRecipesForCalories(List<Recipe> recipes, int targetCalories, int numberOfMeals)
        {
            List<List<Recipe>> allCombinations = GetAllRecipeCombinations(recipes, targetCalories, numberOfMeals);

            // Shuffle the list of combinations to introduce randomness
            allCombinations.Shuffle();

            foreach (var combination in allCombinations)
            {
                int totalCalories = (int)combination.Sum(recipe => recipe.Calories);
                if (totalCalories >= targetCalories - 33 * numberOfMeals && totalCalories <= targetCalories + 33 * numberOfMeals)
                {
                    // Update TempData with total calories
                    TempData["TotalCalories"] = totalCalories;
                    return combination;
                }
            }

            // If no valid combination found, return an empty list
            return new List<Recipe>();
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
