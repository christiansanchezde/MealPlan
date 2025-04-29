using JSONDatabaseManager.Models;

namespace JSONDatabaseManager.Interfaces
{
    public interface IRecipeService
    {
        Task<List<Recipe>> GetAllAsync();
        Task<Recipe?> GetByNameAsync(string name);
        Task AddAsync(Recipe recipe);
        Task UpdateAsync(Recipe recipe);
        Task DeleteAsync(string name);
    }
}
