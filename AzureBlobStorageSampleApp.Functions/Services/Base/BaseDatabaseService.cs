using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AzureBlobStorageSampleApp.Shared;
using Microsoft.EntityFrameworkCore;

namespace AzureBlobStorageSampleApp.Functions
{
    public abstract class BaseDatabaseService
    {
        readonly static string _connectionString = Environment.GetEnvironmentVariable("PhotoDatabaseConnectionString") ?? string.Empty;

        protected static async Task<TResult> PerformDatabaseFunction<TResult>(Func<PhotosContext, TResult> databaseFunction)
        {
            using var connection = new PhotosContext();

            try
            {
                var result = databaseFunction(connection);

                await connection.SaveChangesAsync().ConfigureAwait(false);

                return result;
            }
            catch (Exception e)
            {
                Debug.WriteLine("");
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.ToString());
                Debug.WriteLine("");

                throw;
            }
        }

        protected static async Task<TResult> PerformDatabaseFunction<TResult>(Func<PhotosContext, Task<TResult>> databaseFunction)
        {
            using var connection = new PhotosContext();

            try
            {
                var result = await databaseFunction(connection).ConfigureAwait(false);

                await connection.SaveChangesAsync().ConfigureAwait(false);

                return result;

            }
            catch (Exception e)
            {
                Debug.WriteLine("");
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.ToString());
                Debug.WriteLine("");

                throw;
            }
        }

        protected class PhotosContext : DbContext
        {
            public PhotosContext() => Database.EnsureCreated();

            public DbSet<PhotoModel>? Photos { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlServer(_connectionString);
        }
    }
}
