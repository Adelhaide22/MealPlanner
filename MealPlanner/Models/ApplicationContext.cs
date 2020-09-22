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

            base.OnModelCreating(builder);
        } 
    }
}