using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using JSONDatabaseManager.Interfaces;
using JSONDatabaseManager.Models;
using JSONDatabaseManager.Services;

internal class Program
{
    static async Task Main(string[] args)
    {
        // 1) Build host & DI
        using IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                // point at recipes.json in the working folder
                services.AddSingleton<IRecipeService>(_ =>
                    new RecipeService("recipes.json"));
            })
            .Build();

        var recipeSvc = host.Services.GetRequiredService<IRecipeService>();

        // 2) Report and reset the JSON file
        var dbPath = Path.GetFullPath("recipes.json");
        Console.WriteLine($"Database file location: {dbPath}");
        if (File.Exists(dbPath))
            File.Delete(dbPath);

        // 3) Create three initial recipes
        var recipes = new List<Recipe>
        {
            new Recipe
            {
                Name = "Spaghetti Bolognese",
                Description = "Classic Italian pasta with a rich meat sauce.",
                Preparation = "1) Sauté onions. 2) Brown beef. 3) Add tomato sauce and simmer 20 min. 4) Serve over spaghetti.",
                Ingredients = new List<Ingredient>
                {
                    new Ingredient { Name = "Spaghetti",    Location = "Pasta aisle",   Unit = "g",   Quantity = 200 },
                    new Ingredient { Name = "Ground Beef",  Location = "Meat section",  Unit = "g",   Quantity = 300 },
                    new Ingredient { Name = "Tomato Sauce", Location = "Canned goods",  Unit = "ml",  Quantity = 400 },
                    new Ingredient { Name = "Onion",        Location = "Produce",      Unit = "pcs", Quantity = 1   },
                }
            },
            new Recipe
            {
                Name = "Chicken Salad",
                Description = "Fresh salad with grilled chicken and veggies.",
                Preparation = "1) Grill chicken and slice. 2) Chop veggies. 3) Toss everything with dressing.",
                Ingredients = new List<Ingredient>
                {
                    new Ingredient { Name = "Chicken Breast",  Location = "Meat section", Unit = "g",   Quantity = 250 },
                    new Ingredient { Name = "Lettuce",         Location = "Produce",      Unit = "head",Quantity = 1   },
                    new Ingredient { Name = "Cherry Tomatoes", Location = "Produce",      Unit = "g",   Quantity = 150 },
                    new Ingredient { Name = "Cucumber",        Location = "Produce",      Unit = "pcs", Quantity = 1   },
                }
            },
            new Recipe
            {
                Name = "Pancakes",
                Description = "Fluffy pancakes perfect for breakfast.",
                Preparation = "1) Mix flour, baking powder, milk & eggs. 2) Pour on hot griddle. 3) Flip when bubbles form.",
                Ingredients = new List<Ingredient>
                {
                    new Ingredient { Name = "Flour",         Location = "Baking aisle", Unit = "g",   Quantity = 200 },
                    new Ingredient { Name = "Milk",          Location = "Dairy",        Unit = "ml",  Quantity = 300 },
                    new Ingredient { Name = "Egg",           Location = "Dairy",        Unit = "pcs", Quantity = 2   },
                    new Ingredient { Name = "Baking Powder", Location = "Baking aisle", Unit = "tsp", Quantity = 1   },
                }
            }
        };

        foreach (var r in recipes)
            await recipeSvc.AddAsync(r);

        // 4) Delete one recipe
        await recipeSvc.DeleteAsync("Chicken Salad");
        Console.WriteLine("Deleted recipe: Chicken Salad\n");

        // 5) List remaining recipe names
        Console.WriteLine("=== Remaining Recipes ===");
        var remaining = await recipeSvc.GetAllAsync();
        foreach (var r in remaining)
        {
            Console.WriteLine($"- {r.Name}");
        }
    }
}
