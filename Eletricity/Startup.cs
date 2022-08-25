using Eletricity;
using System.IO;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Eletricity.Configuration;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Eletricity.Helper;
using Eletricity.Data;

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
                .AddEnvironmentVariables().Build();
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {

            // Inject Settings as Options
            builder.Services.AddOptions<SQLSettings>()
                            .Configure<IConfiguration>((settings, configuration) => { configuration.Bind("SQLSettings", settings); });

            builder.Services.AddOptions<ELOverblikSettings>()
                            .Configure<IConfiguration>((settings, configuration) => { configuration.Bind("ELOverblikSettings", settings); });

            builder.Services.AddOptions<BlobStorageSettings>()
                          .Configure<IConfiguration>((settings, configuration) => { configuration.Bind("BlobStorageSettings", settings); });


            builder.Services.AddSingleton<ElOverblikToken>();     
            builder.Services.AddSingleton<Prices>();
            builder.Services.AddSingleton<Metering>();
            builder.Services.AddSingleton<Spotprices>();
            builder.Services.AddSingleton<Tariff>();
            builder.Services.AddSingleton<Prognose>();
            builder.Services.AddSingleton<UploadBlob>();
            builder.Services.AddSingleton<GetBlobData>();
            builder.Services.AddSingleton<InsertData>();
            builder.Services.AddSingleton<SqlConnecter>();
            builder.Services.AddSingleton<SqlExecuteHelper>();

            builder.Services.AddLogging();



        }
    }
}
