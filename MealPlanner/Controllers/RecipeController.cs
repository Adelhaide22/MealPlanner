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
                new Recipe() {RecipeId = 0, Name = "Meat with egg", Categories = new List<string>{"Meat"}, Ingridients = new List<string>(){"Meat","Oil","Egg"}},
                new Recipe() {RecipeId = 1, Name = "Tomato soup", Categories = new List<string>{"Soup"}, Ingridients = new List<string>(){"Tomato","Water","Egg"}},
                new Recipe() {RecipeId = 2, Name = "Fried meat", Categories = new List<string>{"Meat"}, Ingridients = new List<string>(){"Meat","Oil","Pepper"}},
                new Recipe() {RecipeId = 3, Name = "Carrot soup", Categories = new List<string>{"Meat"}, Ingridients = new List<string>(){"Cucumber","Water","Carrot"}},
            };
           _menu = new List<CategoriesMenuViewModel>
            {
                new CategoriesMenuViewModel
                {
                    CategoryName = "Meat",
                    Recipes = _recipes.Where(r=>r.Categories.Contains("Meat"))
                },
                new CategoriesMenuViewModel
                {
                    CategoryName = "Soup",
                    Recipes = _recipes.Where(r=>r.Categories.Contains("Soup"))
                }
            };
        }
        
        public IActionResult GetRecipe(int recipeId)
        {
            ViewBag.Menu = _menu;
            return View(_recipes.ElementAt(recipeId));
        }

        public IActionResult AddRecipe()
        {
            ViewBag.Menu = _menu;
            return View();
        }

        public IActionResult EditRecipe()
        {
            ViewBag.Menu = _menu;
            return View();
        }
        
        [HttpPost]
        [Route("/DeleteRecipe/{recipeId}")]
        public IActionResult DeleteRecipe(int recipeId)
        {
            // foreach (var category in _menu)
            // {
            //     foreach (var recipe in category.Recipes)
            //     {
            //         if (recipe.RecipeId == recipeId)
            //         {
            //             _recipes.Remove(recipe);
            //         }
            //     }
            // }
            ViewBag.Menu = _menu;
            return View();
        }

        [HttpGet]
        [Route("/DeleteRecipe/{recipeId}")]
        public IActionResult ConfirmRecipeDeletion(int recipeId)
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