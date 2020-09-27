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
        public DbSet<Category> Categories { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<RecipeCategory> RecipesCategories { get; set; }
        public DbSet<RecipeIngredient> RecipesIngredients { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<RecipeCategory>()
                .HasKey(rc => new { rc.RecipeId, rc.CategoryId });  
            builder.Entity<RecipeCategory>()
                .HasOne(rc => rc.Recipe)
                .WithMany(r => r.RecipesCategories)
                .HasForeignKey(rc => rc.RecipeId);
            builder.Entity<RecipeCategory>()
                .HasOne(rc => rc.Category)
                .WithMany(c => c.RecipesCategories)
                .HasForeignKey(rc => rc.CategoryId); 
            
            builder.Entity<RecipeIngredient>()
                .HasKey(ri => new { ri.RecipeId, ri.IngredientId });  
            builder.Entity<RecipeCategory>()
                .HasOne(rc => rc.Recipe)
                .WithMany(r => r.RecipesCategories)
                .HasForeignKey(rc => rc.RecipeId);
            builder.Entity<RecipeCategory>()
                .HasOne(rc => rc.Category)
                .WithMany(c => c.RecipesCategories)
                .HasForeignKey(rc => rc.CategoryId); 
            
            base.OnModelCreating(builder);
        } 
    }
}