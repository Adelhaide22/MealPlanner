using System;
using System.Collections.Generic;
using System.Linq;
using MealPlanner.Models.ViewModels;

namespace MealPlanner.Models.ViewModels
{
    public static class Menu
    {
        public static List<CategoriesMenuViewModel> CategoriesMenu = new List<CategoriesMenuViewModel>();

        public static void AddCategory(string categoryName, List<Recipe> recipes)
        {
            var categoriesMenuViewModel = new CategoriesMenuViewModel
            {
                CategoryName = categoryName,
                Recipes = recipes.Where(r => r.Categories.Contains(categoryName))
            };
            CategoriesMenu.Add(categoriesMenuViewModel);
        }

        public static void GenerateMenu(List<Recipe> recipes)
        {
            foreach (var recipe in recipes)
            {
                for (var i = 0; i < recipe.Categories.Count(); i++)
                {
                    if (!CategoriesMenu.Exists(c => c.CategoryName == recipe.Categories[i])
                            || !CategoriesMenu.Exists(c=> c.Recipes.Contains(recipe)))
                    {
                        var categoriesMenuViewModel = new CategoriesMenuViewModel
                        {
                            CategoryName = recipe.Categories[i],
                            Recipes = recipes.Where(r => r.Categories.Contains(recipe.Categories[i])).ToList()
                        };
                        CategoriesMenu.Add(categoriesMenuViewModel);
                    }
                }
            }
        }
    }
}