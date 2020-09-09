using System;
using System.Collections.Generic;
using System.Linq;
using MealPlanner.Models.ViewModels;

namespace MealPlanner.Models.ViewModels
{
    public class NavMenu
    {
        private ApplicationContext db;
        public List<CategoriesMenuViewModel> CategoriesMenuViewModels;
        
        public NavMenu(ApplicationContext context)
        {
            db = context;
            CategoriesMenuViewModels = GenerateMenu(db.Recipes.ToList());
        }
        
        public static List<CategoriesMenuViewModel> GenerateMenu(List<Recipe> recipes)
        {
            var menu = recipes
                .SelectMany(r => r.Categories
                    .Select(c => (c, r)))
                .GroupBy(t => t.c)
                .Select(g =>
                    new CategoriesMenuViewModel
                    {
                        CategoryName = g.Key,
                        Recipes = g.Select(i => i.r).ToList(),
                    })
                .ToList();

            return menu;
        }
    }
}