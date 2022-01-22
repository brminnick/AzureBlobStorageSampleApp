using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
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
                    services.AddLogging();

                    services.AddDbContext<PhotosDbContext>(options => options.UseSqlServer(_connectionString));

                    services.AddSingleton<PhotoDatabaseService>();
                    services.AddSingleton<PhotosBlobStorageService>();
                    services.AddSingleton<BlobServiceClient>(new BlobServiceClient(_storageConnectionString));
                }).Build();

            return host.RunAsync();
        }
    }
}