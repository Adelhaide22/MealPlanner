using System.Linq;
using System.Threading.Tasks;
using MealPlanner.Models;
using MealPlanner.Models.ViewModels;
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
            var recipe = db.Recipes.FirstOrDefault(r => r.RecipeId == recipeId);
            var recipeViewModel = ParseToViewModel(recipe);
            return View(recipeViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddRecipe(RecipeViewModel newRecipe)
        {
            var recipe = ParseFromViewModel(newRecipe);
            
            await db.Recipes.AddAsync(recipe);
            await db.SaveChangesAsync();
            
            return RedirectToAction("Index");
        }
        
        public IActionResult AddRecipe()
        {
            return View();
        }

        public IActionResult EditRecipe(int recipeId)
        {
            var recipe = db.Recipes.FirstOrDefault(r => r.RecipeId == recipeId);
            var recipeViewModel = ParseToViewModel(recipe);
            return View(recipeViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditRecipe(RecipeViewModel editedRecipe)
        {
            var recipe = ParseFromViewModel(editedRecipe);
            
            db.Recipes.Update(recipe);
            await db.SaveChangesAsync();
            
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteRecipe(int recipeId)
        {
            var recipe = await db.Recipes.FirstOrDefaultAsync(r => r.RecipeId == recipeId);
            
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
            var recipe = db.Recipes.FirstOrDefault(r => r.RecipeId == recipeId);
            var recipeViewModel = ParseToViewModel(recipe);
            return View(recipeViewModel);
        }

        public IActionResult Index()
        {
            return View();
        }

        private RecipeViewModel ParseToViewModel(Recipe recipe)
        {
            return new RecipeViewModel
            {
                RecipeId = recipe.RecipeId,
                Name = recipe.Name,
                Categories = string.Join(", ", recipe.Categories),
                Ingredients = string.Join(", ", recipe.Ingredients),
                Instructions = recipe.Instructions
            };
        }
        
        private Recipe ParseFromViewModel(RecipeViewModel recipeViewModel)
        {
            return new Recipe
            {
                RecipeId = recipeViewModel.RecipeId,
                Name = recipeViewModel.Name,
                Categories = recipeViewModel.Categories.Split(", ").ToList(),
                Ingredients = recipeViewModel.Ingredients.Split(", ").ToList(),
                Instructions = recipeViewModel.Instructions
            };
        }
    }
}