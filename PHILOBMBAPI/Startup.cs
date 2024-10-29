using PHILOBMCore.Services.Interfaces;
using PHILOBMCore.Services;
using Serilog;
using Serilog.Events;
using PHILOBMCore.Database;
using PHILOBMCore.ConstantsSettings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;

        // Configurer Serilog au moment de la construction de Startup
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Error()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
            .Enrich.FromLogContext()
            .WriteTo.File("logs/app.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();
    }

    // Configure les services ici
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(); 
        });

        AddServices(services);

        AddDbContextRelative(services);
    }

    // Enregistrement des services spécifiques
    private void AddServices(IServiceCollection services)
    {
        services.AddSingleton(provider => new FileService
        {
            DatabaseFileName = Constants.DBName,
            BackupDirectory = Constants.BackupFolderName,
            MaxBackupCount = Constants.MaxBackupCount,
            ShowMessageBoxes = Constants.ShowMessageBoxes
        });

        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<ICarService, CarService>();
        services.AddScoped<IInvoiceService, InvoiceService>();
    }

    // Configurer le contexte de base de données avec SQLite
    private static void AddDbContextRelative(IServiceCollection services)
    {
        Outils.CréerDossierSiInexistant(Constants.RacinePath);

        services.AddDbContext<PhiloBMContext>(options =>
            options.UseSqlite($"Data Source={Constants.DbPath}"));
    }

    // Configure le pipeline HTTP ici
    public void Configure(WebApplication app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
    }
}
