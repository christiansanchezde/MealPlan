using Serilog;
using System.Diagnostics;

namespace MealPlanner.Services;

public static class ServiceExtensions
{
    public static MauiAppBuilder ConfigureServices(this MauiAppBuilder builder)
    {
        Logging.Configure(); //Call serilog implementation of static function

        Log.Debug("Configuring Services ");
        Log.Verbose("Initializing database");
        try
        {            
            RecipeService dbService = new RecipeService();            
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Database Initialization failed. Exception: " + ex);
        }

        Log.Verbose("Adding database service to builder");
        builder.Services.AddSingleton<RecipeService>();

        Log.Debug("Services configuration Complete");
        // Services add 
        return builder;
    }
}
