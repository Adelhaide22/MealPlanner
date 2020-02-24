using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MealPlanner.Models
{
    public class Recipe
    {
        public string Name { get; set; }
        public List<string> Category { get; set; }
        public List<string> Ingridients { get; set; }
        public string Instructions { get; set; }
    }
}
