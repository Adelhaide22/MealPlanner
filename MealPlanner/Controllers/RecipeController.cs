using System.Collections.Generic;
using System.Linq;
using MealPlanner.Models;
using MealPlanner.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MealPlanner.Controllers
{
    public class RecipeController : Controller
    {
        private IList<CategoriesMenuViewModel> _menu;
        private IList<Recipe> _recipes;
        
        public RecipeController()
        {
            _recipes = new List<Recipe>()
            {
                new Recipe() {RecipeId = 0, Name = "Meat with egg", Categories = new List<string>{"Meat"}, Ingredients = new List<string>(){"Meat","Oil","Egg"}},
                new Recipe() {RecipeId = 1, Name = "Tomato soup", Categories = new List<string>{"Soup"}, Ingredients = new List<string>(){"Tomato","Water","Egg"}},
                new Recipe() {RecipeId = 2, Name = "Fried meat", Categories = new List<string>{"Meat"}, Ingredients = new List<string>(){"Meat","Oil","Pepper"}},
                new Recipe() {RecipeId = 3, Name = "Carrot soup", Categories = new List<string>{"Soup"}, Ingredients = new List<string>(){"Cucumber","Water","Carrot"}},
            };
           _menu = new List<CategoriesMenuViewModel>
            {
                new CategoriesMenuViewModel
                {
                    CategoryName = "Meat",
                    Recipes = _recipes.Where(r=>r.Categories.Contains("Meat")).ToList()
                },
                new CategoriesMenuViewModel
                {
                    CategoryName = "Soup",
                    Recipes = _recipes.Where(r=>r.Categories.Contains("Soup")).ToList()
                }
            };
        }
        
        public IActionResult GetRecipe(int recipeId)
        {
            ViewBag.Menu = _menu;
            return View(_recipes.ElementAt(recipeId));
        }

        [HttpPost]
        public IActionResult AddRecipe(Recipe newRecipe)
        {
            newRecipe.RecipeId = _recipes.Count;
            newRecipe.Categories = newRecipe.Categories[0].Split(('\n')).ToList();
            newRecipe.Ingredients = newRecipe.Ingredients[0].Split(('\n')).ToList();
            _recipes.Add(newRecipe);
            ViewBag.Menu = _menu;
            return RedirectToAction("Index");
        }
        
        public IActionResult AddRecipe()
        {
            ViewBag.Menu = _menu;
            return View();
        }

        public IActionResult EditRecipe(int recipeId)
        {
            ViewBag.Menu = _menu;
            return View(_recipes.ElementAt(recipeId));
        }
        
        [HttpPost]
        public IActionResult EditRecipe(Recipe editedRecipe)
        {
            if (_recipes.Contains(editedRecipe))
            {
                _recipes[editedRecipe.RecipeId] = editedRecipe;
            }
            ViewBag.Menu = _menu;
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public IActionResult DeleteRecipe(int recipeId)
        {
            foreach (var category in _menu)
            {
                foreach (var recipe in category.Recipes)
                {
                    if (recipe.RecipeId == recipeId)
                    {
                        _recipes.Remove(recipe);
                    }
                }
            }
            
            return RedirectToAction("Index");
        }

        [ActionName("DeleteRecipe")]
        public IActionResult ConfirmDeletion(int recipeId)
        {
            ViewBag.Menu = _menu;
            return View(_recipes.ElementAt(recipeId));
        }


        public IActionResult Index()
        {
            ViewBag.Menu = _menu;
            return View();
        }
    }
}