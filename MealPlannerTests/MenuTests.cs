using System.Collections.Generic;
using FluentAssertions;
using MealPlanner;
using MealPlanner.Models;
using MealPlanner.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace MealPlannerTests
{
    public class MenuTests
    {
        [Fact]
        public void GenerateMenu_DifferentRecipesAdded_DifferentCategoriesCreated()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "Test")
                .Options;
            
            var context = new ApplicationContext(options);
            
            var recipe1 = new Recipe()
            {
                Categories = new List<string>() {"1", "2"}, Ingredients = new List<string>() {"cheese"},
                Instructions = "just cook and eat", Name = "a", RecipeId = 1
            };
            var recipe2 = new Recipe()
            {
                Categories = new List<string>() {"1"}, Ingredients = new List<string>() {"cheese"},
                Instructions = "just cook and eat", Name = "b", RecipeId = 2
            };
            var recipe3 = new Recipe()
            {
                Categories = new List<string>() {"2"}, Ingredients = new List<string>() {"cheese"},
                Instructions = "just cook and eat", Name = "c", RecipeId = 3
            };
            
            context.Recipes.AddRange(recipe1, recipe2, recipe3);
            
            var navMenu = new NavMenu(context);
            navMenu.CategoriesMenuViewModels.Count.Should().Be(2);
        }
    }
}