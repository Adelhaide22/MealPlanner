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
        private Random rnd;
        private DbContextOptions<ApplicationContext> options;
        private ApplicationContext context;
        private NavMenu navMenu;
        private RecipeController recipeController;
        
        public RecipeControllerTests()
        {
            rnd = new Random();
            options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: rnd.Next().ToString())
                .Options;
            
            context = new ApplicationContext(options);
            
            navMenu = new NavMenu(context);
            recipeController = new RecipeController(context);
            
            var recipe1 = new Recipe
            {
                Categories = new List<string> {"1", "2"}, Ingredients = new List<string> {"cheese"},
                Instructions = "just cook and eat", Name = "a", RecipeId = 1
            };
            var recipe2 = new Recipe
            {
                Categories = new List<string> {"1"}, Ingredients = new List<string> {"cheese"},
                Instructions = "just cook and eat", Name = "b", RecipeId = 2
            };
            var recipe3 = new Recipe
            {
                Categories = new List<string> {"2"}, Ingredients = new List<string> {"cheese"},
                Instructions = "just cook and eat", Name = "c", RecipeId = 3
            };
            context.Recipes.AddRange(recipe1, recipe2, recipe3);
            context.SaveChangesAsync();
        }
        
        [Fact]
        public void AddRecipe_NewCategory_NewCategoryCreated()
        {
            var newRecipe = new RecipeViewModel()
            {
                Categories = "3", Ingredients = "cheese",
                Instructions = "just cook and eat", Name = "d", RecipeId = 4
            };

            recipeController.AddRecipe(newRecipe);

            navMenu.CategoriesMenuViewModels = NavMenu.GenerateMenu(context.Recipes.ToList());
            
            context.Recipes.Count().Should().Be(4);
            navMenu.CategoriesMenuViewModels.Count.Should().Be(3);
        }
        
        [Fact]
        public void AddRecipe_ExistingCategories_RecipeAddedToExistingCategories()
        {
            var newRecipe = new RecipeViewModel()
            {
                Categories = "1, 2", Ingredients = "cheese",
                Instructions = "just cook and eat", Name = "d", RecipeId = 4
            };

            recipeController.AddRecipe(newRecipe);

            navMenu.CategoriesMenuViewModels = NavMenu.GenerateMenu(context.Recipes.ToList());
            
            context.Recipes.Count().Should().Be(4);
            navMenu.CategoriesMenuViewModels.Count.Should().Be(2);
        }
        
        [Fact]
        public void EditRecipe_CategoryChanged_RecipeUpdated()
        {
            var newRecipe = new RecipeViewModel()
            {
                Categories = "1, 2",
                Ingredients = "cheese",
                Instructions = "just cook and eat",
                Name = "aaaa",
                RecipeId = 1,
            };

            var newContext = new ApplicationContext(options);
            recipeController = new RecipeController(newContext);
            
            recipeController.EditRecipe(newRecipe).Wait();

            navMenu.CategoriesMenuViewModels = NavMenu.GenerateMenu(newContext.Recipes.ToList());
            
            newContext.Recipes.Count().Should().Be(3);
            
            newContext.Recipes.First(r => r.RecipeId == 1).Name.Should().Be("aaaa");
            navMenu.CategoriesMenuViewModels.Count.Should().Be(2);
        }
        
        [Fact]
        public void DeleteRecipe_RecipeDeleted_RecipeIsNotExists()
        {
            recipeController.DeleteRecipe(3);

            navMenu.CategoriesMenuViewModels = NavMenu.GenerateMenu(context.Recipes.ToList());
            
            context.Recipes.Count().Should().Be(2);
            navMenu.CategoriesMenuViewModels.Count.Should().Be(2);
        }
        
        [Fact]
        public void DeleteRecipe_RecipeWithCategoryDeleted_CategoryDeleted()
        {
            recipeController.DeleteRecipe(1);
            recipeController.DeleteRecipe(3);

            navMenu.CategoriesMenuViewModels = NavMenu.GenerateMenu(context.Recipes.ToList());
            
            context.Recipes.Count().Should().Be(1);
            navMenu.CategoriesMenuViewModels.Count.Should().Be(1);
        }
    }
}