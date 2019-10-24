using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureBlobStorageSampleApp.Shared;
using NPoco;

namespace AzureBlobStorageSampleApp.Functions
{
    public abstract class PhotoDatabaseService : BaseDatabaseService
    {
        public static List<PhotoModel> GetAllPhotos() => GetAllPhotos(x => true);

        public static List<PhotoModel> GetAllPhotos(Func<PhotoModel, bool> wherePredicate)
        {
            return PerformDatabaseFunction(getAllPhotos);

            List<PhotoModel> getAllPhotos(Database dataContext)
            {
                var photoList = dataContext.Fetch<PhotoModel>().Where(wherePredicate).ToList();
                return photoList;
            }
        }

        public static Task<PhotoModel> InsertPhoto(PhotoModel photo)
        {
            return PerformDatabaseFunction(insertPhoto);

            async Task<PhotoModel> insertPhoto(Database dataContext)
            {
                if (string.IsNullOrWhiteSpace(photo.Id))
                    photo.Id = Guid.NewGuid().ToString();

                var currentTime = DateTimeOffset.UtcNow;

                photo.CreatedAt = currentTime;
                photo.UpdatedAt = currentTime;

                await dataContext.InsertAsync(photo).ConfigureAwait(false);

                return photo;
            }
        }

        public static Task<PhotoModel> UpdatePhoto(PhotoModel photo)
        {
            return PerformDatabaseFunction(updatePhoto);

            async Task<PhotoModel> updatePhoto(Database dataContext)
            {
                var photoFromDatabase = dataContext.Fetch<PhotoModel>().Single(y => y.Id.Equals(photo.Id));

                photoFromDatabase.IsDeleted = photo.IsDeleted;
                photoFromDatabase.Title = photo.Title;
                photoFromDatabase.Url = photo.Url;
                photoFromDatabase.UpdatedAt = DateTimeOffset.UtcNow;

                await dataContext.UpdateAsync(photoFromDatabase).ConfigureAwait(false);

                return photoFromDatabase;
            }
        }

        public static Task<PhotoModel> DeletePhoto(string id)
        {
            return PerformDatabaseFunction(removePunModelDatabaseFunction);

            async Task<PhotoModel> removePunModelDatabaseFunction(Database dataContext)
            {
                var photoFromDatabase = dataContext.Fetch<PhotoModel>().Single(x => x.Id.Equals(id));

                await dataContext.DeleteAsync(photoFromDatabase).ConfigureAwait(false);

                return photoFromDatabase;
            }
        }
    }
}
