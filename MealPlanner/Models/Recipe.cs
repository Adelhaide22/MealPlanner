using System.Collections.Generic;

namespace MealPlanner.Models
{
    public class Recipe
    {
        public int RecipeId { get; set; }
        public string Name { get; set; }
        public List<string> Categories { get; set; }
        public List<string> Ingredients { get; set; }
        public string Instructions { get; set; }
    }
}
