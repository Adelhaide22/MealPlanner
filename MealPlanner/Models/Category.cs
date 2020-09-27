using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MealPlanner.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required]
        public string CategoryName { get; set; }
        public List<RecipeCategory> RecipesCategories { get; set; }
    }
}