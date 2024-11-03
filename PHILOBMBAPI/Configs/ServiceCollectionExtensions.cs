using PHILOBMBusiness.Services.Interfaces;
using PHILOBMBusiness.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Configuration;
using PHILOBMDatabase.Database;
using Configuration.ConstantsSettings;
using PHILOBMDatabase.Repositories.Interfaces;
using PHILOBMDatabase.Repositories;
using PHILOBMCore;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using FluentValidation.AspNetCore;
using PHILOBMBAPI.Validators;

namespace PHILOBMBAPI.Configs;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                builder => builder.AllowAnyOrigin()
                                  .AllowAnyMethod()
                                  .AllowAnyHeader());
        });
        return services;
    }

    public static IServiceCollection AddCustomValidators(this IServiceCollection services)
    {
        //services.AddControllers()
        //        .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CarValidator>());

        //services.AddFluentValidationAutoValidation();

        return services;
    }

    public static IServiceCollection AddCustomControllers(this IServiceCollection services)
    {
        services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                });
        return services;
    }

    public static IServiceCollection AddSwaggerGenWithJwt(this IServiceCollection services)
    {
        return services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "PHILOBMBAPI", Version = "v1" });
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

    public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
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
                ValidIssuer = configuration["ConfigurationSettings:Jwt:Issuer"],
                ValidAudience = configuration["ConfigurationSettings:Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["ConfigurationSettings:Jwt:Secret"]))
            };
        });
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<ICarService, CarService>();
        services.AddScoped<IInvoiceService, InvoiceService>();
        services.AddScoped<IPdfService, PdfService>();
        services.AddScoped<IExcellService, ExcellService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IUserService, UserService>();
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<ICarRepository, CarRepository>();
        services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }

    public static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ConfigurationSettings>(configuration.GetSection("ConfigurationSettings"));
        services.AddSingleton(sp =>
            sp.GetRequiredService<IOptions<ConfigurationSettings>>().Value);
        return services;
    }

    public static IServiceCollection AddDbContextRelative(this IServiceCollection services)
    {
        Outils.CréerDossierSiInexistant(Constants.RacinePath);

        services.AddDbContext<PhiloBMContext>(options =>
            options.UseSqlite($"Data Source={Constants.DbPath}",
            b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name))); // Utiliser le nom de l'assembly actuel
        return services;
    }
}
