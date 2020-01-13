using AzureBlobStorageSampleApp.Functions;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;


[assembly: FunctionsStartup(typeof(Startup))]
namespace AzureBlobStorageSampleApp.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddLogging();

            builder.Services.AddSingleton<PhotoDatabaseService>();
            builder.Services.AddSingleton<PhotosBlobStorageService>();
        }
    }
}