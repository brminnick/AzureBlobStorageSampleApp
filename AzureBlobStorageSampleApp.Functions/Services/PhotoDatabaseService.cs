using System;
using System.Linq;
using System.Diagnostics;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;

using AzureBlobStorageSampleApp.Shared;

using Microsoft.Extensions.Logging;

using NPoco;

namespace AzureBlobStorageSampleApp.Functions
{
    public abstract class PhotoDatabaseService : BaseDatabaseService
    {
        #region Methods
        public static List<PhotoModel> GetAllPhotos() => GetAllPhotos(x => true);

        public static List<PhotoModel> GetAllPhotos(Func<PhotoModel, bool> wherePredicate)
        {
            return PerformDatabaseFunction(getAllPhotos).GetAwaiter().GetResult();

            Task<List<PhotoModel>> getAllPhotos(Database dataContext)
            {
                var photoList = dataContext.Fetch<PhotoModel>().Where(wherePredicate).ToList();
                return Task.FromResult(photoList);
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
                var photoFromDatabase = dataContext.Fetch<PhotoModel>().Where(y => y.Id.Equals(photo.Id)).FirstOrDefault();

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
                var photoFromDatabase = dataContext.Fetch<PhotoModel>().Where(x => x.Id.Equals(id)).First();

                await dataContext.DeleteAsync(photoFromDatabase).ConfigureAwait(false);

                return photoFromDatabase;
            }
        }
        #endregion
    }
}
