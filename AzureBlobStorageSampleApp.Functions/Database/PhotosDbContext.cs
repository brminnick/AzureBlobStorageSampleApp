using AzureBlobStorageSampleApp.Shared;
using Microsoft.EntityFrameworkCore;

namespace AzureBlobStorageSampleApp.Functions
{
    class PhotosDbContext : DbContext
    {
        public PhotosDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<PhotoModel> Photos => Set<PhotoModel>();
    }
}
