namespace JSONDatabaseManager.Models
{
    public class Ingredient
    {
        public required string Name { get; set; }
        public required string Location { get; set; }
        public required string Unit { get; set; }
        public required double Quantity { get; set; }
    }
}
