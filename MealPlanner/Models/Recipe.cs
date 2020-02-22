using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MealPlanner.Models
{
    public class Recipe
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public List<string> Ingridients { get; set; }
        public string Instruction { get; set; }
    }
}
