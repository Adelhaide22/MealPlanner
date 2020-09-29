using System.Collections.Generic;
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
            var recipe = db.Recipes
                .Include(r => r.RecipesCategories)
                    .ThenInclude(rc => rc.Category)
                .Include(r => r.RecipesIngredients)
                    .ThenInclude(ri => ri.Ingredient)
                .FirstOrDefault(r => r.RecipeId == recipeId);
            var recipeViewModel = ConvertToViewModel(recipe);
            return View(recipeViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddRecipe(RecipeViewModel newRecipe)
        {
            var recipe = ConvertFromViewModel(newRecipe);
            
            await db.Recipes.AddAsync(recipe);
            await db.RecipesCategories.AddRangeAsync(recipe.RecipesCategories);
            await db.RecipesIngredients.AddRangeAsync(recipe.RecipesIngredients);
            await db.SaveChangesAsync();
            
            return RedirectToAction("Index");
        }
        
        public IActionResult AddRecipe()
        {
            return View();
        }

        public IActionResult EditRecipe(int recipeId)
        {
            var recipe = db.Recipes
                .Include(r => r.RecipesCategories)
                    .ThenInclude(rc => rc.Category)
                .Include(r => r.RecipesIngredients)
                    .ThenInclude(ri => ri.Ingredient)
                .FirstOrDefault(r => r.RecipeId == recipeId);
            var recipeViewModel = ConvertToViewModel(recipe);
            return View(recipeViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditRecipe(RecipeViewModel editedRecipe)
        {
            var recipe = ConvertFromViewModel(editedRecipe);
            
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
            var recipeViewModel = ConvertToViewModel(recipe);
            return View(recipeViewModel);
        }

        public IActionResult Index()
        {
            return View();
        }

        private RecipeViewModel ConvertToViewModel(Recipe recipe)
        {
            return new RecipeViewModel
            {
                RecipeId = recipe.RecipeId,
                Name = recipe.Name,
                Categories = string.Join(", ", recipe.RecipesCategories.FirstOrDefault(r => r.CategoryId == recipe.RecipeId)),
                Ingredients = string.Join(", ", recipe.RecipesIngredients.FirstOrDefault(r => r.IngredientId == recipe.RecipeId)),
                Instructions = recipe.Instructions
            };
        }
        
        private Recipe ConvertFromViewModel(RecipeViewModel recipeViewModel)
        {
            var categories = recipeViewModel.Categories.Split(", ").ToList();
            var categoryIds = new List<int>();
            foreach (var c in categories)
            {
                var id = db.RecipesCategories
                    .FirstOrDefault(rc => rc.Category.CategoryName == c)?.CategoryId 
                         ?? categoryIds.Count + 1;
                categoryIds.Add(id);
            }
            var recipesCategories = new List<RecipeCategory>();
            foreach (var id in categoryIds)
            {
                db.Categories.AddAsync(new Category
                {
                    CategoryName = categories[id - 1]
                });
                db.SaveChanges();
                recipesCategories.Add(new RecipeCategory
                    {
                        RecipeId = recipeViewModel.RecipeId,
                        CategoryId = id,
                    }
                );
            }

            var ingredients = recipeViewModel.Ingredients.Split(", ").ToList();
            var ingredientIds = new List<int>();
            foreach (var i in ingredients)
            {
                var id = db.RecipesIngredients
                    .FirstOrDefault(ri => ri.Ingredient.IngredientName == i)?.IngredientId
                    ?? ingredientIds.Count + 1;
                ingredientIds.Add(id);
            }
            var recipesIngredients = new List<RecipeIngredient>();
            foreach (var id in ingredientIds)
            {
                db.Ingredients.AddAsync(new Ingredient
                {
                    IngredientName = ingredients[id-1],
                });
                db.SaveChanges();
                recipesIngredients.Add(new RecipeIngredient
                    {
                        RecipeId = recipeViewModel.RecipeId,
                        IngredientId = id,
                    }
                );
            }
            
            return new Recipe
            {
                RecipeId = recipeViewModel.RecipeId,
                Name = recipeViewModel.Name,
                RecipesCategories = recipesCategories,
                RecipesIngredients = recipesIngredients,
                Instructions = recipeViewModel.Instructions
            };
        }
    }
}