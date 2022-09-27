using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using PricingCalculator.Calculation;
using PricingCalculator.Data;
using System.Runtime.CompilerServices;

internal class Program
{
    private static void Main(string[] args)
    {
        var host = new HostBuilder()
        .ConfigureAppConfiguration(config =>
        {
            var environmentName = Environment.GetEnvironmentVariable("FUNCTIONS_ENVIRONMENT");
            if (environmentName.IsNullOrWhiteSpace())
            {
                config.AddJsonFile("local.settings.json", optional: true, reloadOnChange: false);
            }
            config.AddEnvironmentVariables();
        })
        .ConfigureFunctionsWorkerDefaults(worker =>
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            worker.UseNewtonsoftJson(jsonSerializerSettings);
        })
        .ConfigureOpenApi()
        .ConfigureServices(ConfigureDependencyInjection)
        .Build();

        host.Run();
    }

    private static void ConfigureDependencyInjection(HostBuilderContext hostBuilder, IServiceCollection services)
    {
        services.AddLogging();
        services.AddTransient<ICalculationService, CalculationService>();
        services.AddSingleton<IRepository, Repository>();
    }
}