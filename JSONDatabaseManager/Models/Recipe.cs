using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONDatabaseManager.Models
{
    public class Recipe
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Preparation { get; set; }
        public required List<Ingredient> Ingredients { get; set; }
    }
}
