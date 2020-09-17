namespace MealPlanner.Models.ViewModels
{
    public class RecipeViewModel
    {
        public int RecipeId { get; set; }
        public string Name { get; set; }
        public string Categories { get; set; }
        public string Ingredients { get; set; }
        public string Instructions { get; set; }
    }
}