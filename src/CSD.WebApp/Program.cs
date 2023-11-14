using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Threading.Tasks;

namespace CSD.WebApp;

public static class Program
{
    private static Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureApp<Startup>(args)
            .Build();

        return host.RunAsync();
    }
}

public static class HostBuilderExtension
{
    public static IHostBuilder ConfigureApp<TStartup>(this IHostBuilder hostBuilder, string[] args) where TStartup : class {
        hostBuilder.ConfigureAppConfiguration((context, configBuilder) => {
            configBuilder.Sources.Clear();
            var env = context.HostingEnvironment.EnvironmentName;

            configBuilder.AddJsonFile("appsettings.json");
            configBuilder.AddJsonFile($"Configs/connectionStrings.{env}.json");

            configBuilder.AddEnvironmentVariables();

            if (args is not null) {
                configBuilder.AddCommandLine(args);
            }
        });

        hostBuilder.ConfigureWebHostDefaults(webBuilder => {
            webBuilder.UseStartup<TStartup>();
            //.UseKestrel(options => {
            //    options.Listen(new IPAddress(new byte[] { 192, 168, 3, 8 }), 5197);
            //});
        });

        return hostBuilder;
    }
}