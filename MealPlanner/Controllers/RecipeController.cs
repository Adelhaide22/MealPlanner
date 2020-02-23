using System.Collections.Generic;
using MealPlanner.Models;
using Microsoft.AspNetCore.Mvc;

namespace MealPlanner.Controllers
{
    public class RecipeController : Controller
    {
        // GET
        public IActionResult Index()
        {
            var recipe = new Recipe() {Name = "Meat", Ingridients = new List<string>(){"Meat","Oil","Egg"}};
            return View(recipe);
        }
    }
}