using System.Collections.Immutable;

namespace MealPlanner.Models.ViewModels
{
    public class NavMenuViewModel
    {
        public ImmutableList<Category> Categories { get; }
        
        public NavMenuViewModel(ImmutableList<Category> categories)
        {
            Categories = categories;
        }
    }
}