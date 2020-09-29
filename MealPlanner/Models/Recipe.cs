using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MealPlanner.Models
{
    public class Recipe
    {
        [Key]
        public int RecipeId { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        public List<RecipeCategory> RecipesCategories { get; set; }
        public List<RecipeIngredient> RecipesIngredients { get; set; }
        public string Instructions { get; set; }
    }
}
