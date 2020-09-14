using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using MealPlanner;
using MealPlanner.Controllers;
using MealPlanner.Models;
using MealPlanner.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace MealPlannerTests
{
    public class RecipeControllerTests
    {
        [Fact]
        public void AddRecipe_NewCategoryAdded_NewCategoryCreated()
        {
            var rnd = new Random();
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: rnd.Next().ToString())
                .Options;
            
            var context = new ApplicationContext(options);

            var newRecipe = new Recipe()
            {
                Categories = new List<string>() {"3"}, Ingredients = new List<string>() {"cheese"},
                Instructions = "just cook and eat", Name = "d", RecipeId = 4
            };
            
            var navMenu = new NavMenu(context);
            var recipeController = new RecipeController(context);

            recipeController.AddRecipe(newRecipe);

            navMenu.CategoriesMenuViewModels = NavMenu.GenerateMenu(context.Recipes.ToList());
            
            context.Recipes.Count().Should().Be(4);
            navMenu.CategoriesMenuViewModels.Count.Should().Be(3);
        }
    }
}