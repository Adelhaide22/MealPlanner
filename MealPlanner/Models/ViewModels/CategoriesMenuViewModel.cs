using System.Collections.Generic;

namespace MealPlanner.Models.ViewModels
{
    public class CategoriesMenuViewModel
    {
        public string CategoryName { get; set; }
        public IEnumerable<Recipe> Recipes { get; set; }
    }
}