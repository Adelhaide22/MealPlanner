using System;
using System.Linq;
using System.Threading.Tasks;
using MealPlanner.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MealPlanner.Controllers
{
    public class RecipeController : Controller
    {
        private ApplicationContext db;
        
        public RecipeController(ApplicationContext context)
        {
            db = context;
        }
        
        public IActionResult GetRecipe(int recipeId)
        {
            return View(db.Recipes.FirstOrDefault(r => r.RecipeId == recipeId));
        }

        [HttpPost]
        public async Task<IActionResult> AddRecipe(Recipe newRecipe)
        {
            newRecipe.Categories = newRecipe.Categories[0].Split('\r' + Environment.NewLine).ToList();
            newRecipe.Ingredients = newRecipe.Ingredients[0].Split('\r' + Environment.NewLine).ToList();
            
            await db.Recipes.AddAsync(newRecipe);
            await db.SaveChangesAsync();
            
            return RedirectToAction("Index");
        }
        
        public IActionResult AddRecipe()
        {
            return View();
        }

        public IActionResult EditRecipe(int recipeId)
        {
            return View(db.Recipes.FirstOrDefault(r => r.RecipeId == recipeId));
        }
        
        [HttpPost]
        public async Task<IActionResult> EditRecipe(Recipe editedRecipe)
        {
            db.Recipes.Update(editedRecipe);
            await db.SaveChangesAsync();
            
            //update menu if categories were changed
            
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteRecipe(int recipeId)
        {
            var recipe = await db.Recipes.FirstOrDefaultAsync(r => r.RecipeId == recipeId);
            
            // update menu
            
            if (recipe != null)
            {
                db.Recipes.Remove(recipe);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return NotFound();
        }

        [ActionName("DeleteRecipe")]
        public IActionResult ConfirmDeletion(int recipeId)
        {
            return View(db.Recipes.FirstOrDefault(r => r.RecipeId == recipeId));
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}