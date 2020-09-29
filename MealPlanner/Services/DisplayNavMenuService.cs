using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using MealPlanner.Models;
using MealPlanner.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

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

        public List<RecipeCategory> GetRecipesForCategory(Category category)
        {
            return db.RecipesCategories
                .Include(rc => rc.Recipe)
                .Where(rc => rc.CategoryId == category.CategoryId)
                .ToList();
        }
    }
}