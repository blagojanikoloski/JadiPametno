using JadiPametno.Models;
using JadiPametno.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

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

        [HttpGet]
        [Route("recipe/Add")]
        public IActionResult Add()
        {
            var allIngredients = _context.Ingredient.ToList();
            TempData["AllIngredients"] = allIngredients;
            return View("~/Views/Recipe/Add.cshtml");
        }


        [HttpPost]
        public IActionResult Add(RecipeAddDto model)
        {

            // Capitalize the first letter and make the rest lowercase for RecipeName
            if (!string.IsNullOrWhiteSpace(model.RecipeName))
            {
                model.RecipeName = char.ToUpper(model.RecipeName[0]) + model.RecipeName.Substring(1).ToLower();
            }

            System.Diagnostics.Debug.WriteLine("Name: " + model.RecipeName);
            System.Diagnostics.Debug.WriteLine("Type: " + model.RecipeType);
            if (model.SelectedIngredients != null)
            {
                foreach (var ingredientId in model.SelectedIngredients)
                {
                    System.Diagnostics.Debug.WriteLine("Ingredient ID: " + ingredientId);
                    
                }
            }
            System.Diagnostics.Debug.WriteLine("Calories: " + model.Calories);
            System.Diagnostics.Debug.WriteLine("Instructions: " + model.Instructions);
            System.Diagnostics.Debug.WriteLine("Image URL: " + model.ImageUrl);

            var recipeToAdd = new Recipe
            {
                Name = model.RecipeName,
                Calories = model.Calories,
                Description = model.Instructions,
                ImageUrl = model.ImageUrl,
                Type = model.RecipeType,
            };
            _context.Recipe.Add(recipeToAdd);
            _context.SaveChanges();

            
            int newRecipeId = recipeToAdd.RecipeId;
            foreach (var ingredientId in model.SelectedIngredients)
            {
                var RecipeHasIngredientToAdd = new RecipeHasIngredient
                {
                    RecipeId = newRecipeId,
                    IngredientId = ingredientId,
                };

                _context.RecipeHasIngredient.Add(RecipeHasIngredientToAdd);
            }
            _context.SaveChanges();

            return Redirect("/");
        }


        [HttpPost]
        public async Task<IActionResult> AddIngredient(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Ingredient name cannot be empty.");
            }

            // Capitalize the first letter and make the rest lowercase
            name = char.ToUpper(name[0]) + name.Substring(1).ToLower();

            // Check if the ingredient already exists
            var existingIngredient = await _context.Ingredient.FirstOrDefaultAsync(i => i.IngredientName == name);

            if (existingIngredient != null)
            {
                // Ingredient already exists, return conflict status
                return Conflict("Ingredient already exists");
            }

            // Ingredient does not exist, add it to the database
            var newIngredient = new Ingredient { IngredientName = name };

            _context.Ingredient.Add(newIngredient);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }


    }
}

