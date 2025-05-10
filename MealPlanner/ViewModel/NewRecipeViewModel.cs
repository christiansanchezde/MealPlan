using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Serilog;
using MealPlanner.Model;
using MealPlanner.Services;
using System.Threading.Tasks.Dataflow;
using MealPlanner.Interfaces;

namespace MealPlanner.ViewModel;

public partial class NewRecipeViewModel : BaseViewModel
{
    private readonly IRecipeService _databaseService;

    public NewRecipeViewModel(RecipeService databaseService)
    {
        Title = "Create new recipe";
        _databaseService = databaseService;

        Log.Debug("New Recipe view model called");
        if (_databaseService == null)
        {
            Log.Error("The database Service is null on NewRecipeViewModel");
        }

        // Seed with a demo ingredient
        Ingredients = new ObservableCollection<Ingredient>
        {
            new("Flour", SupermarketSection.Brot_Bakery , Unit.grams, 100),
            new("Sugar", SupermarketSection.Brot_Bakery, Unit.grams, 200),
            new("Salt",  SupermarketSection.Other, Unit.grams, 300)
        };

        // Load your ingredient list from the DB/service
        IngredientList = new ObservableCollection<string>(
            //_databaseService.GetAllIngredientNames()
        );
    }

    #region Bindable Properties

    [ObservableProperty]
    private string recipeName;

    [ObservableProperty]
    private string portions;

    [ObservableProperty]
    private string preparation;

    public ObservableCollection<Ingredient> Ingredients { get; set; }

    [ObservableProperty]
    private ObservableCollection<string> ingredientList = new();

    [ObservableProperty]
    private string addIngredientName;

    [ObservableProperty]
    private string addIngredientUnit;

    [ObservableProperty]
    private string addIngredientQuantity;

    #endregion

    #region Commands

    [RelayCommand]
    void AddNewIngredient()
    {
        //// you may want to lookup unit from database/service instead of hard-coding
        //if (string.IsNullOrWhiteSpace(AddIngredientName)
        //    || !float.TryParse(AddIngredientQuantity, out var qty))
        //    return;

        //Ingredients.Add(new IngredientInfo(AddIngredientName, Unit.grams, qty));

        //// clear inputs
        //AddIngredientName = string.Empty;
        //AddIngredientQuantity = string.Empty;
    }

    [RelayCommand]
    void RemoveIngredient(Ingredient ingredient)
    {
        if (Ingredients.Contains(ingredient))
            Ingredients.Remove(ingredient);
    }

    [RelayCommand]
    void AddNewIngredientType()
    {
        // stub: later open a dialog or navigate to "create ingredient" page
    }

    [RelayCommand]
    void SaveRecipe()
    {
        // stub: persist RecipeName, Portions, Preparation, Ingredients to your database
    }

    #endregion
}
