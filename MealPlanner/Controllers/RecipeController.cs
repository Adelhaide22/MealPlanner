using System.Collections.Generic;
using MealPlanner.Models;
using MealPlanner.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MealPlanner.Controllers
{
    public class RecipeController : Controller
    {
        // GET
        public IActionResult Index()
        {
            var recipeMeat = new Recipe() {Name = "Meat with egg", Ingridients = new List<string>(){"Meat","Oil","Egg"}};
            var recipeSoup = new Recipe() {Name = "Tomato soup", Ingridients = new List<string>(){"Tomato","Water","Egg"}};
            var menu = new List<CategoriesMenuViewModel>
            {
                new CategoriesMenuViewModel
                 {
                      CategoryName = "Meat",
                      Recipes = new List<Recipe>() {recipeMeat, recipeSoup}
                 }
            };
            ViewBag.Menu = menu;
            return View(recipeMeat);
        }
    }
}