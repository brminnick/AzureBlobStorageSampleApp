using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureBlobStorageSampleApp.Shared;
using Microsoft.EntityFrameworkCore;

namespace AzureBlobStorageSampleApp.Functions
{
    class PhotoDatabaseService
    {
        readonly PhotosDbContext _photosDbContext;

        public PhotoDatabaseService(PhotosDbContext photosDbContext) => _photosDbContext = photosDbContext;

        public async Task<IReadOnlyList<PhotoModel>> GetAllPhotos() =>
            await _photosDbContext.Photos.ToListAsync().ConfigureAwait(false);

        public IReadOnlyList<PhotoModel> GetAllPhotos(Func<PhotoModel, bool> wherePredicate) =>
            _photosDbContext.Photos.Where(wherePredicate).ToList();

        public async Task<PhotoModel> InsertPhoto(PhotoModel photo)
        {
            if (string.IsNullOrWhiteSpace(photo.Id))
                photo = photo with { Id = Guid.NewGuid().ToString() };

            var currentTime = DateTimeOffset.UtcNow;

            photo = photo with
            {
                CreatedAt = currentTime,
                UpdatedAt = currentTime
            };

            await _photosDbContext.AddAsync(photo).ConfigureAwait(false);
            await _photosDbContext.SaveChangesAsync().ConfigureAwait(false);

            return photo;
        }

        public async Task<PhotoModel> UpdatePhoto(PhotoModel photo)
        {
            var photoFromDatabase = _photosDbContext.Photos.Single(y => y.Id.Equals(photo.Id));

            var updatedPhoto = photoFromDatabase with
            {
                IsDeleted = photo.IsDeleted,
                Title = photo.Title,
                Url = photo.Url,
                UpdatedAt = DateTimeOffset.UtcNow
            };

            _photosDbContext.Update(updatedPhoto);
            await _photosDbContext.SaveChangesAsync().ConfigureAwait(false);

            return photoFromDatabase;
        }

        public async Task<PhotoModel> DeletePhoto(string id)
        {
            var photoFromDatabase = _photosDbContext.Photos.Single(x => x.Id.Equals(id));

            _photosDbContext.Remove(photoFromDatabase);
            await _photosDbContext.SaveChangesAsync().ConfigureAwait(false);

            return photoFromDatabase;
        }
    }
}
