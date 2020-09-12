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
        private static Random rnd = new Random();
        
        private List<Recipe> recipes = new List<Recipe>()
        {
            new Recipe()
            {
                Categories = new List<string>() {"1", "2"}, Ingredients = new List<string>() {"cheese"},
                Instructions = "just cook and eat", Name = "a", RecipeId = 1
            },
            new Recipe()
            {
                Categories = new List<string>() {"1"}, Ingredients = new List<string>() {"cheese"},
                Instructions = "just cook and eat", Name = "b", RecipeId = 2
            },
            new Recipe()
            {
                Categories = new List<string>() {"2"}, Ingredients = new List<string>() {"cheese"},
                Instructions = "just cook and eat", Name = "c", RecipeId = 3
            }
        };

        
        public void Te()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "1")
                .Options;
            
            var context = new ApplicationContext(options);
            context.Recipes.RemoveRange(context.Recipes);
            context.SaveChanges();
        }
        
        [Fact]
        public void AddRecipe_NewCategoryAdded_NewCategoryCreated()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: rnd.Next().ToString())
                .Options;
            
            var context = new ApplicationContext(options);
            context.Recipes.RemoveRange(context.Recipes);
            context.SaveChanges();

            var newRecipe = new Recipe()
            {
                Categories = new List<string>() {"3"}, Ingredients = new List<string>() {"cheese"},
                Instructions = "just cook and eat", Name = "d", RecipeId = 4
            };
            
            context.Recipes.AddRange(recipes);
            context.SaveChanges();

            var navMenu = new NavMenu(context);
            var recipeController = new RecipeController(context);

            recipeController.AddRecipe(newRecipe);

            context.Recipes.Count().Should().Be(4);
            navMenu.CategoriesMenuViewModels.Count.Should().Be(3);
        }
    }
}