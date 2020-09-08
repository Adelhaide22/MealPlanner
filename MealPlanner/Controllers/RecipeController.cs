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
            Menu.GenerateMenu(db.Recipes.ToList());

            //  _recipes = new List<Recipe>()
            //  {
            //      new Recipe() {RecipeId = 0, Name = "Meat with egg", Categories = new List<string>{"Meat"}, Ingredients = new List<string>(){"Meat","Oil","Egg"}},
            //      new Recipe() {RecipeId = 1, Name = "Tomato soup", Categories = new List<string>{"Soup"}, Ingredients = new List<string>(){"Tomato","Water","Egg"}},
            //      new Recipe() {RecipeId = 2, Name = "Fried meat", Categories = new List<string>{"Meat"}, Ingredients = new List<string>(){"Meat","Oil","Pepper"}},
            //      new Recipe() {RecipeId = 3, Name = "Carrot soup", Categories = new List<string>{"Soup"}, Ingredients = new List<string>(){"Cucumber","Water","Carrot"}},
            //  };
        }
        
        public IActionResult GetRecipe(int recipeId)
        {
            ViewBag.Menu = Menu.CategoriesMenu;
            return View(db.Recipes.FirstOrDefault(r => r.RecipeId == recipeId));
        }

        [HttpPost]
        public async Task<IActionResult> AddRecipe(Recipe newRecipe)
        {
            newRecipe.Categories = newRecipe.Categories[0].Split(('\n')).ToList();
            newRecipe.Categories.ForEach(s => s.Trim('\r'));
            newRecipe.Ingredients = newRecipe.Ingredients[0].Split(('\n')).ToList();
            newRecipe.Ingredients.ForEach(s => s.Trim('\r'));
            
            await db.Recipes.AddAsync(newRecipe);
            await db.SaveChangesAsync();

            foreach (var category in newRecipe.Categories)
            {
                if (!Menu.CategoriesMenu.Any())
                {
                    Menu.AddCategory(category, db.Recipes.ToList());
                    continue;
                }

                var newCategories = Menu.CategoriesMenu.FindAll(cm => cm.CategoryName != category);

                if (newCategories.Count() < newRecipe.Categories.Count())
                {
                    Menu.AddCategory(category, db.Recipes.ToList());
                }
            }

            ViewBag.Menu = Menu.CategoriesMenu;
            return RedirectToAction("Index");
        }
        
        public IActionResult AddRecipe()
        {
            ViewBag.Menu = Menu.CategoriesMenu;
            return View();
        }

        public IActionResult EditRecipe(int recipeId)
        {
            ViewBag.Menu = Menu.CategoriesMenu;
            return View(db.Recipes.FirstOrDefault(r => r.RecipeId == recipeId));
        }
        
        [HttpPost]
        public async Task<IActionResult> EditRecipe(Recipe editedRecipe)
        {
            db.Recipes.Update(editedRecipe);
            await db.SaveChangesAsync();
            
            ViewBag.Menu = Menu.CategoriesMenu;
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
            ViewBag.Menu = Menu.CategoriesMenu;
            return View(db.Recipes.FirstOrDefault(r => r.RecipeId == recipeId));
        }


        public IActionResult Index()
        {
            ViewBag.Menu = Menu.CategoriesMenu;
            return View();
        }
    }
}