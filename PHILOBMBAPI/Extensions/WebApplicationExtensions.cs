using Microsoft.AspNetCore.Localization;
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
}