using PHILOBMCore.Services.Interfaces;
using PHILOBMCore.Services;
using Serilog;
using Serilog.Events;
using PHILOBMCore.Database;
using PHILOBMCore.ConstantsSettings;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.Extensions.Options;
using Configuration;

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
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                builder => builder.AllowAnyOrigin()
                                  .AllowAnyMethod()
                                  .AllowAnyHeader());
        });

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

    // Configure le pipeline HTTP ici
    public void Configure(WebApplication app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                //c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
                //c.RoutePrefix = string.Empty; // Pour afficher Swagger à la racine
                c.DocExpansion(DocExpansion.None);
            });
        }

        app.UseCors("AllowAllOrigins");
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.MapControllers();
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

        AddConfiguration(services);
    }

    private void AddConfiguration(IServiceCollection services)
    {
        services.Configure<ConfigurationSettings>(_configuration.GetSection("ConfigurationSettings"));

        services.AddSingleton(sp =>
            sp.GetRequiredService<IOptions<ConfigurationSettings>>().Value);
    }

    // Configurer le contexte de base de données avec SQLite
    private static void AddDbContextRelative(IServiceCollection services)
    {
        Outils.CréerDossierSiInexistant(Constants.RacinePath);

        services.AddDbContext<PhiloBMContext>(options =>
            options.UseSqlite($"Data Source={Constants.DbPath}"));
    }
}
