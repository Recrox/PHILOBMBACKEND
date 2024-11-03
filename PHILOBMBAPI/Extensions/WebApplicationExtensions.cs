using Hangfire;
using Microsoft.AspNetCore.Localization;
using Serilog;
using System.Globalization;

namespace PHILOBMBAPI.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication AddMiddlewares(this WebApplication app)
    {
        //app.UseMiddleware<ModelValidationMiddleware>();
        return app;

    }
    public static WebApplication AddCultureInfo(this WebApplication app)
    {
        var defaultCulture = new CultureInfo("fr-FR");
        var localizationOptions = new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture(defaultCulture),
            SupportedCultures = new List<CultureInfo> { defaultCulture },
            SupportedUICultures = new List<CultureInfo> { defaultCulture }
        };

        app.UseRequestLocalization(localizationOptions);

        //// Vérifier la configuration du fuseau horaire
        //var timeZoneId = app.Configuration["AppSettings:TimeZone"] ?? "Romance Standard Time"; // Fuseau horaire par défaut
        //TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

        //// Vous pouvez stocker le fuseau horaire pour l'utiliser plus tard
        //app.Services.AddSingleton(localTimeZone);

        return app;
    }

    public static WebApplication AddHangfire(this WebApplication app)
    {
        app.UseHangfireDashboard(); // Pour accéder à l'interface de Hangfire (facultatif)

        // Démarrez le job de Hangfire
        RecurringJob.AddOrUpdate("daily-job", () => YourMethodToExecute(), Cron.Daily(3, 0)); // S'exécute tous les jours à 3 heures du matin
        return app;
    }

    private static void YourMethodToExecute()
    {
        Log.Information("Le job a été exécuté avec succès à {Time}", DateTime.UtcNow);
    }
}