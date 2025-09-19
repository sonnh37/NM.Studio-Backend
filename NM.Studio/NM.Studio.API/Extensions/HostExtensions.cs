using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace NM.Studio.API.Extensions;

public static class HostExtensions
{
    public static void AddAppConfiguration(this IHostBuilder host)
    {
        host.ConfigureAppConfiguration((context, builder) =>
        {
            var env = context.HostingEnvironment;
            builder
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, false)
                .AddEnvironmentVariables();
        });
    }

    public static void AddSerilog(this IHostBuilder host, IConfiguration appConfiguration)
    {
        host.UseSerilog((context, services, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(appConfiguration)
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level:u3} U:{Username} C:{LogCategory}] [TraceId: {TraceId}] {Message:lj}{NewLine}{Exception}",
                    theme: AnsiConsoleTheme.Code);
        });
    }
}