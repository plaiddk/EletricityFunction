using Eletricity;
using System.IO;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Eletricity.Configuration;
using Microsoft.Azure.WebJobs.Host.Bindings;


[assembly: FunctionsStartup(typeof(Startup))]

namespace Eletricity
{
   
    public class Startup : FunctionsStartup
    {
        // Override ConfigureAppConfiguration method to inject appsettings json files
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            FunctionsHostBuilderContext context = builder.GetContext();

            builder.ConfigurationBuilder
                .SetBasePath(context.ApplicationRootPath)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: false)
                .AddJsonFile("settings.json", optional: true, reloadOnChange: false)
                .AddEnvironmentVariables();
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {

            // Inject Settings as Options
            builder.Services.AddOptions<ConnectionSettings>()
                            .Configure<IConfiguration>((settings, configuration) => { configuration.Bind("ConnectionStrings", settings); });

            builder.Services.AddOptions<ELoverblikAccess>()
                            .Configure<IConfiguration>((settings, configuration) => { configuration.Bind("ELoverblikAccess", settings); });




        }
    }
}
