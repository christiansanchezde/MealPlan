namespace MealPlanner.Model
{
    public class Ingredient
    {
        public string Name { get; set; }
        public SupermarketSection Location { get; set; }
        public Unit Unit { get; set; }
        public double Quantity { get; set; }

        public Ingredient (string name, SupermarketSection location, Unit unit, double quantity)
        {
            Name = name;
            Location = location;
            Unit = unit;
            Quantity = quantity;
        }

    }
    
}
