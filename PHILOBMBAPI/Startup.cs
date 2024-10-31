using PHILOBMBusiness.Services.Interfaces;
using PHILOBMBusiness.Services;
using Serilog;
using Serilog.Events;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.Extensions.Options;
using Configuration;
using PHILOBMDatabase.Database;
using Configuration.ConstantsSettings;
using PHILOBMDatabase.Repositories.Interfaces;
using PHILOBMDatabase.Repositories;
using PHILOBMBAPI;
using PHILOBMCore;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
        AddLogs();
    }

    private static void AddLogs()
    {
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

        services.AddAutoMapper(typeof(MappingProfile).Assembly);

        AddServices(services);
        AddRepositories(services);

        AddConfiguration(services);
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
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<ICarService, CarService>();
        services.AddScoped<IInvoiceService, InvoiceService>();
        services.AddScoped<IPdfService, PdfService>();
        services.AddScoped<IExcellService, ExcellService>();
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<ICarRepository, CarRepository>();
        services.AddScoped<IInvoiceRepository, InvoiceRepository>();
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
