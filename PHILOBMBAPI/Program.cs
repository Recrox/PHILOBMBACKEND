internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Appelle Startup pour la configuration
        var startup = new Startup(builder.Configuration);
        startup.ConfigureServices(builder.Services); // Configure les services

        var app = builder.Build();

        startup.Configure(app, app.Environment); // Configure le pipeline

        app.Run();
    }
}