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
using PHILOBMCore;
using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

namespace PHILOBMBAPI;
public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
        AddLogs();
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
        app.UseMiddleware<ModelValidationMiddleware>();

        app.UseCors("AllowAllOrigins");
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication(); // Ajoutez ceci pour l'authentification
        app.UseAuthorization();
        app.MapControllers();
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
        AddSwaggerGen(services);

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
        AddValidators(services);

        //eviter les references circulaires
        services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });

        AddAuthentification(services);
    }

    private IServiceCollection AddSwaggerGen(IServiceCollection services)
    {
        return services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "PHILOBMBAPI", Version = "v1" });

            // Configuration pour ajouter le support JWT
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Entrez 'Bearer' suivi d'un espace et du token JWT. Exemple: 'Bearer 12345abcdef'",
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }


    private void AddAuthentification(IServiceCollection services)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
           .AddJwtBearer(options =>
           {
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = true,
                   ValidateAudience = true,
                   ValidateLifetime = true,
                   ValidateIssuerSigningKey = true,
                   ValidIssuer = _configuration["ConfigurationSettings:Jwt:Issuer"],
                   ValidAudience = _configuration["ConfigurationSettings:Jwt:Audience"],
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["ConfigurationSettings:Jwt:Secret"]))
               };
           });
    }

    private void AddValidators(IServiceCollection services)
    {
        //services.AddControllers()
        //        .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CarValidator>());

        //services.AddFluentValidationAutoValidation();
    }

    // Configure le pipeline HTTP ici
    

    // Enregistrement des services spécifiques
    private void AddServices(IServiceCollection services)
    {
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<ICarService, CarService>();
        services.AddScoped<IInvoiceService, InvoiceService>();
        services.AddScoped<IPdfService, PdfService>();
        services.AddScoped<IExcellService, ExcellService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IUserService, UserService>();
    }

    private void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<ICarRepository, CarRepository>();
        services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
    }

    private void AddConfiguration(IServiceCollection services)
    {
        services.Configure<ConfigurationSettings>(_configuration.GetSection("ConfigurationSettings"));

        services.AddSingleton(sp =>
            sp.GetRequiredService<IOptions<ConfigurationSettings>>().Value);
    }

    // Configurer le contexte de base de données avec SQLite
    private void AddDbContextRelative(IServiceCollection services)
    {
        Outils.CréerDossierSiInexistant(Constants.RacinePath);

        services.AddDbContext<PhiloBMContext>(options =>
            options.UseSqlite($"Data Source={Constants.DbPath}",
            b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name))); // Utiliser le nom de l'assembly actuel
    }

}
