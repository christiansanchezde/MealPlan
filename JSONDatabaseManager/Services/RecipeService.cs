using System.Text.Json;
using JSONDatabaseManager.Interfaces;
using JSONDatabaseManager.Models;

namespace JSONDatabaseManager.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly string _filePath;
        private readonly JsonSerializerOptions _opts = new() { WriteIndented = true };
        private readonly SemaphoreSlim _mutex = new(1, 1);

        public RecipeService(string filePath = "recipes.json")
            => _filePath = filePath;

        private async Task<List<Recipe>> LoadAsync()
        {
            if (!File.Exists(_filePath)) return new();
            var json = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<List<Recipe>>(json, _opts)
                   ?? new();
        }

        private async Task SaveAsync(List<Recipe> list)
            => await File.WriteAllTextAsync(_filePath, JsonSerializer.Serialize(list, _opts));

        public async Task<List<Recipe>> GetAllAsync()
        {
            await _mutex.WaitAsync();
            try { return await LoadAsync(); }
            finally { _mutex.Release(); }
        }

        public async Task<Recipe?> GetByNameAsync(string name)
            => (await GetAllAsync()).Find(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        public async Task AddAsync(Recipe recipe)
        {
            await _mutex.WaitAsync();
            try
            {
                var list = await LoadAsync();
                if (list.Any(r => r.Name == recipe.Name))
                    throw new InvalidOperationException($"Recipe '{recipe.Name}' exists.");
                list.Add(recipe);
                await SaveAsync(list);
            }
            finally { _mutex.Release(); }
        }

        public async Task UpdateAsync(Recipe recipe)
        {
            await _mutex.WaitAsync();
            try
            {
                var list = await LoadAsync();
                var idx = list.FindIndex(r => r.Name == recipe.Name);
                if (idx < 0) throw new KeyNotFoundException();
                list[idx] = recipe;
                await SaveAsync(list);
            }
            finally { _mutex.Release(); }
        }

        public async Task DeleteAsync(string name)
        {
            await _mutex.WaitAsync();
            try
            {
                var list = await LoadAsync();
                var r = list.Find(rp => rp.Name == name)
                        ?? throw new KeyNotFoundException();
                list.Remove(r);
                await SaveAsync(list);
            }
            finally { _mutex.Release(); }
        }
    }
}
