using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

// --- DEMO PROGRAM ---
// Instantiate repository and report file location
var repo = new RecipeRepository("recipes.json");

// Start clean
if (File.Exists("recipes.json"))
    File.Delete("recipes.json");

// 1) Create three recipes
var initialRecipes = new List<Recipe>
{
    new Recipe
    {
        Name = "Spaghetti Bolognese",
        Description = "Classic Italian pasta with a rich meat sauce.",
        Preparation = "1) Sauté onions and garlic. 2) Add beef and brown. 3) Stir in tomato sauce and simmer 20 min. 4) Serve over spaghetti.",
        Ingredients = new List<Ingredient>
        {
            new Ingredient { Name = "Spaghetti",    Location = "Pasta aisle",   Unit = "g",    Quantity = 200 },
            new Ingredient { Name = "Ground Beef",  Location = "Meat section",  Unit = "g",    Quantity = 300 },
            new Ingredient { Name = "Tomato Sauce", Location = "Canned goods",  Unit = "ml",   Quantity = 400 },
            new Ingredient { Name = "Onion",        Location = "Produce",      Unit = "pcs",  Quantity = 1   },
        }
    },
    new Recipe
    {
        Name = "Chicken Salad",
        Description = "Fresh salad with grilled chicken and veggies.",
        Preparation = "1) Grill chicken and slice. 2) Chop lettuce, tomatoes, cucumber. 3) Toss everything with dressing.",
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
        Preparation = "1) Mix flour, baking powder, milk, and eggs. 2) Pour batter on hot griddle. 3) Flip when bubbles form.",
        Ingredients = new List<Ingredient>
        {
            new Ingredient { Name = "Flour",         Location = "Baking aisle", Unit = "g",   Quantity = 200  },
            new Ingredient { Name = "Milk",          Location = "Dairy",        Unit = "ml",  Quantity = 300  },
            new Ingredient { Name = "Egg",           Location = "Dairy",        Unit = "pcs", Quantity = 2    },
            new Ingredient { Name = "Baking Powder", Location = "Baking aisle", Unit = "tsp", Quantity = 1    },
        }
    }
};
foreach (var r in initialRecipes)
    repo.Add(r);

// 2) Show initial recipes
Console.WriteLine("=== Initial Recipes ===");
PrintAll(repo);

// 3) Delete one
Console.WriteLine("Deleting 'Chicken Salad'...\n");
repo.Delete("Chicken Salad");

// 4) Add a new one
var guac = new Recipe
{
    Name = "Guacamole",
    Description = "Creamy avocado dip with lime and cilantro.",
    Preparation = "1) Mash avocados. 2) Stir in lime juice, onion, cilantro. 3) Season with salt.",
    Ingredients = new List<Ingredient>
    {
        new Ingredient { Name = "Avocado",  Location = "Produce", Unit = "pcs", Quantity = 2   },
        new Ingredient { Name = "Lime",     Location = "Produce", Unit = "pcs", Quantity = 1   },
        new Ingredient { Name = "Onion",    Location = "Produce", Unit = "pcs", Quantity = 0.5 },
        new Ingredient { Name = "Cilantro", Location = "Produce", Unit = "tbsp",Quantity = 2   },
    }
};
Console.WriteLine("Adding 'Guacamole'...\n");
repo.Add(guac);

// 5) Show final recipes
Console.WriteLine("=== Final Recipes ===");
PrintAll(repo);


// --- HELPERS ---
static void PrintAll(RecipeRepository repo)
{
    foreach (var r in repo.GetAll())
    {
        Console.WriteLine($"* {r.Name} — {r.Description}");
        Console.WriteLine($"  Preparation: {r.Preparation}");
        foreach (var i in r.Ingredients)
            Console.WriteLine($"    - {i.Name}: {i.Quantity} {i.Unit} (Section: {i.Location})");
        Console.WriteLine();
    }
}

// --- MODELS ---
public class Ingredient
{
    public string Name { get; set; }
    public string Location { get; set; }
    public string Unit { get; set; }
    public double Quantity { get; set; }
}

public class Recipe
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Preparation { get; set; }
    public List<Ingredient> Ingredients { get; set; }
}

// --- REPOSITORY ---
public class RecipeRepository
{
    private readonly string _filePath;
    private List<Recipe> _recipes;
    private readonly JsonSerializerOptions _opts = new() { WriteIndented = true };

    public RecipeRepository(string filePath)
    {
        _filePath = filePath;
        // Report the database file location
        Console.WriteLine($"Database file location: {Path.GetFullPath(_filePath)}");

        if (File.Exists(_filePath))
        {
            var json = File.ReadAllText(_filePath);
            _recipes = JsonSerializer.Deserialize<List<Recipe>>(json, _opts) ?? new();
        }
        else
        {
            _recipes = new();
        }
    }

    private void Save()
        => File.WriteAllText(_filePath, JsonSerializer.Serialize(_recipes, _opts));

    public List<Recipe> GetAll()
        => _recipes;

    public Recipe? GetByName(string name)
        => _recipes.Find(r => r.Name == name);

    public void Add(Recipe recipe)
    {
        if (GetByName(recipe.Name) is not null)
            throw new InvalidOperationException($"Recipe '{recipe.Name}' already exists.");
        _recipes.Add(recipe);
        Save();
    }

    public void Update(Recipe recipe)
    {
        var idx = _recipes.FindIndex(r => r.Name == recipe.Name);
        if (idx < 0) throw new KeyNotFoundException($"Recipe '{recipe.Name}' not found.");
        _recipes[idx] = recipe;
        Save();
    }

    public void Delete(string name)
    {
        var r = GetByName(name);
        if (r is null) throw new KeyNotFoundException($"Recipe '{name}' not found.");
        _recipes.Remove(r);
        Save();
    }
}
