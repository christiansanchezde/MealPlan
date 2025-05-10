namespace MealPlanner.Model
{
    public class Recipe
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Preparation { get; set; }
        public required List<Ingredient> Ingredients { get; set; }

        public Recipe(string name, string description, string preparation, List<Ingredient> ingredients)
        {
            Name = name;
            Description = description;
            Preparation = preparation;
            Ingredients = ingredients ?? new List<Ingredient>();
        }
    }
}
