using Serilog;
using Serilog.Events;
using Swashbuckle.AspNetCore.SwaggerUI;
using PHILOBMBAPI.Extensions;
using Hangfire;

namespace PHILOBMBAPI;
public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
        AddLogs();
    }
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
                c.InjectStylesheet("/css/swagger-dark.css"); // Ajoute le CSS personnalisé pour le mode sombre
                c.DocExpansion(DocExpansion.None);
            });
        }
        app.AddMiddlewares();
        app.AddCultureInfo();
        app.UseCors("AllowAllOrigins");
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication(); // Ajoutez ceci pour l'authentification
        app.UseAuthorization();
        app.MapControllers();

        //app.UseIpRateLimiting();//eviter le spam de l'api
        //app.AddHangfire();
    }

    


    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCustomCors();
        //services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGenWithJwt();

        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog();
        });

        services.AddAutoMapper(typeof(MappingProfile).Assembly);

        services.AddServices();
        services.AddRepositories();
        services.AddConfigurationSettings(_configuration);
        services.AddDbContextRelative(_configuration);

        services.AddCustomValidators();
        services.AddCustomControllers();
        services.AddCustomAuthentication(_configuration);
        //services.AddLimitedCallOnApi(_configuration);

        //services.AddHangfire();
    }

    

    private void AddLogs()
    {
        // Configurer Serilog au moment de la construction de Startup
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Error()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
            .Enrich.FromLogContext()
            .WriteTo.File("logs/app.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();
    }
}
