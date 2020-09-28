using System.Collections.Immutable;
using MealPlanner.Models.ViewModels;

namespace MealPlanner.Services
{
    public class DisplayNavMenuService
    {
        private ApplicationContext db;

        public DisplayNavMenuService(ApplicationContext context)
        {
            db = context;
        }
        public NavMenuViewModel GetNavMenu()
        {
            return new NavMenuViewModel(db.Categories.ToImmutableList());
        }
    }
}