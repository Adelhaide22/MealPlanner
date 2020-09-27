using System.Collections.Generic;
using System.Linq;

namespace MealPlanner.Models.ViewModels
{
    public class NavMenuViewModel
    {
        public List<Category> Categories;
        private ApplicationContext db;
        
        public NavMenuViewModel(ApplicationContext context)
        {
            db = context;
            Categories = db.Categories.ToList();
        }
    }
}