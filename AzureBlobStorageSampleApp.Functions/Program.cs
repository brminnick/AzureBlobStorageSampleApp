using System;
using System.Threading.Tasks;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AzureBlobStorageSampleApp.Functions
{
    class Program
    {
        readonly static string _connectionString = Environment.GetEnvironmentVariable("PhotoDatabaseConnectionString") ?? string.Empty;
        readonly static string _storageConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage") ?? string.Empty;

        static Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureAppConfiguration(configurationBuilder => configurationBuilder.AddCommandLine(args))
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(services =>
                {
                    // Add Logging
                    services.AddLogging();

                    // Add HttpClient
                    services.AddHttpClient();

                    // Add DbContext
                    services.AddDbContext<PhotosDbContext>(options => options.UseSqlServer(_connectionString));

                    // Add Services
                    services.AddScoped<PhotoDatabaseService>();
                    services.AddSingleton<PhotosBlobStorageService>();
                    services.AddSingleton<CloudBlobClient>(CloudStorageAccount.Parse(_storageConnectionString).CreateCloudBlobClient());
                })
                .Build();

            return host.RunAsync();
        }
    }
}
