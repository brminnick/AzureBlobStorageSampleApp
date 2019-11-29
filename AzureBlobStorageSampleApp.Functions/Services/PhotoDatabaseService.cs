using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureBlobStorageSampleApp.Shared;

namespace AzureBlobStorageSampleApp.Functions
{
    public abstract class PhotoDatabaseService : BaseDatabaseService
    {
        public static Task<List<PhotoModel>> GetAllPhotos() => GetAllPhotos(x => true);

        public static Task<List<PhotoModel>> GetAllPhotos(Func<PhotoModel, bool> wherePredicate)
        {
            return PerformDatabaseFunction(getAllPhotos);

            List<PhotoModel> getAllPhotos(PhotosContext dataContext)
            {
                var photoList = dataContext.Photos.Where(wherePredicate).ToList();
                return photoList;
            }
        }

        public static Task<PhotoModel> InsertPhoto(PhotoModel photo)
        {
            return PerformDatabaseFunction(insertPhoto);

            async Task<PhotoModel> insertPhoto(PhotosContext dataContext)
            {
                if (string.IsNullOrWhiteSpace(photo.Id))
                    photo.Id = Guid.NewGuid().ToString();

                var currentTime = DateTimeOffset.UtcNow;

                photo.CreatedAt = currentTime;
                photo.UpdatedAt = currentTime;

                await dataContext.AddAsync(photo).ConfigureAwait(false);

                return photo;
            }
        }

        public static Task<PhotoModel> UpdatePhoto(PhotoModel photo)
        {
            return PerformDatabaseFunction(updatePhoto);

            PhotoModel updatePhoto(PhotosContext dataContext)
            {
                var photoFromDatabase = dataContext.Photos.Single(y => y.Id.Equals(photo.Id));

                photoFromDatabase.IsDeleted = photo.IsDeleted;
                photoFromDatabase.Title = photo.Title;
                photoFromDatabase.Url = photo.Url;
                photoFromDatabase.UpdatedAt = DateTimeOffset.UtcNow;

                dataContext.Update(photoFromDatabase);

                return photoFromDatabase;
            }
        }

        public static Task<PhotoModel> DeletePhoto(string id)
        {
            return PerformDatabaseFunction(removePunModelDatabaseFunction);

            PhotoModel removePunModelDatabaseFunction(PhotosContext dataContext)
            {
                var photoFromDatabase = dataContext.Photos.Single(x => x.Id.Equals(id));

                dataContext.Remove(photoFromDatabase);

                return photoFromDatabase;
            }
        }
    }
}
