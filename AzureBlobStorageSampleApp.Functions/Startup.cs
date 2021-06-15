using System;
using AzureBlobStorageSampleApp.Functions;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


[assembly: FunctionsStartup(typeof(Startup))]
namespace AzureBlobStorageSampleApp.Functions
{
    public class Startup : FunctionsStartup
    {
        readonly static string _connectionString = Environment.GetEnvironmentVariable("PhotoDatabaseConnectionString") ?? string.Empty;

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddLogging();

            builder.Services.AddDbContext<PhotosDbContext>(options => options.UseSqlServer(_connectionString));

            builder.Services.AddSingleton<PhotoDatabaseService>();
            builder.Services.AddSingleton<PhotosBlobStorageService>();
        }
    }
}