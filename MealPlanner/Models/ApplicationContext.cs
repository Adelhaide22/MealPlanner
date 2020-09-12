using System.Collections.Generic;
using System.Linq;
using MealPlanner.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MealPlanner
{
    public sealed class ApplicationContext : DbContext
    {
        public DbSet<Recipe> Recipes { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // string -> List<string> converter
            var splitStringConverter = new ValueConverter<List<string>, string>(v => string.Join(";", v),
                v => v.Split(new[] { ';' }).ToList());
            builder.Entity<Recipe>().Property(nameof(Recipe.Categories)).HasConversion(splitStringConverter);
            builder.Entity<Recipe>().Property(nameof(Recipe.Ingredients)).HasConversion(splitStringConverter);

            //data seed
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
            
            //  _recipes = new List<Recipe>()
            //  {
            //      new Recipe() {RecipeId = 0, Name = "Meat with egg", Categories = new List<string>{"Meat"}, Ingredients = new List<string>(){"Meat","Oil","Egg"}},
            //      new Recipe() {RecipeId = 1, Name = "Tomato soup", Categories = new List<string>{"Soup"}, Ingredients = new List<string>(){"Tomato","Water","Egg"}},
            //      new Recipe() {RecipeId = 2, Name = "Fried meat", Categories = new List<string>{"Meat"}, Ingredients = new List<string>(){"Meat","Oil","Pepper"}},
            //      new Recipe() {RecipeId = 3, Name = "Carrot soup", Categories = new List<string>{"Soup"}, Ingredients = new List<string>(){"Cucumber","Water","Carrot"}},
            //  };
            
            builder.Entity<Recipe>().HasData(recipe1, recipe2, recipe3);

            base.OnModelCreating(builder);
        } 
    }
}