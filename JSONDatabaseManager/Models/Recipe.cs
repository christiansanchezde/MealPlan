using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONDatabaseManager.Models
{
    public class Recipe
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Preparation { get; set; }
        public List<Ingredient> Ingredients { get; set; }
    }
}
