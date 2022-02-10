using Microsoft.Extensions.Configuration;
using Serilog;

namespace Wbv.WhatsappDigester.Logging;

internal static class Logger
{

    public static void ConfigureLog()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        Log.Logger = new LoggerConfiguration().MinimumLevel.Verbose()
            .Enrich.FromLogContext()
            .ReadFrom.Configuration(builder)
            .CreateLogger();
    }
}