using System;
using System.Linq;
using System.Data.Linq;
using System.Diagnostics;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Http.OData;
using System.Threading.Tasks;
using System.Collections.Generic;

using AzureBlobStorageSampleApp.Shared;

namespace AzureBlobStorageSampleApp.Backend.Common
{
    public abstract class PhotoDatabaseService : BaseDatabaseService
    {
        #region Constant Fields
        readonly static string _connectionString = ConfigurationManager.ConnectionStrings["PhotoDatabaseConnectionString"].ConnectionString;
        #endregion

        #region Methods
        public static Task<IList<PhotoModel>> GetAllPhotos()
        {
            Func<DataContext, IList<PhotoModel>> getAllPhotosFunction = dataContext => dataContext.GetTable<PhotoModel>().ToList();
            return PerformDatabaseFunction(getAllPhotosFunction, _connectionString);
        }

        public static Task<PhotoModel> InsertPhoto(PhotoModel photo)
        {
            Func<DataContext, PhotoModel> insertPhotoFunction = dataContext =>
            {
                if (string.IsNullOrWhiteSpace(photo.Id))
                    photo.Id = Guid.NewGuid().ToString();

                var currentTime = DateTimeOffset.UtcNow;

                photo.CreatedAt = currentTime;
                photo.UpdatedAt = currentTime;

                dataContext.GetTable<PhotoModel>().InsertOnSubmit(photo);

                return photo;
            };

            return PerformDatabaseFunction(insertPhotoFunction, _connectionString);
        }

        public static Task<PhotoModel> PatchPhoto(PhotoModel photo)
        {
            var photoModelDelta = new Delta<PhotoModel>();

            photoModelDelta.TrySetPropertyValue(nameof(PhotoModel.Url), photo.Url);
            photoModelDelta.TrySetPropertyValue(nameof(PhotoModel.IsDeleted), photo.IsDeleted);
            photoModelDelta.TrySetPropertyValue(nameof(PhotoModel.Title), photo.Title);

            return PatchPhoto(photo.Id, photoModelDelta);
        }

        public static Task<PhotoModel> PatchPhoto(string id, Delta<PhotoModel> photo)
        {
            Func<DataContext, PhotoModel> patchPhotoFunction = dataContext =>
            {
                var photoFromDatabase = dataContext.GetTable<PhotoModel>().Where(x => x.Id.Equals(id)).FirstOrDefault();

                photo.Patch(photoFromDatabase);
                photoFromDatabase.UpdatedAt = DateTimeOffset.UtcNow;

                return photoFromDatabase;
            };

            return PerformDatabaseFunction(patchPhotoFunction, _connectionString);
        }

        public static Task<PhotoModel> DeletePhoto(string id)
        {
            Func<DataContext, PhotoModel> deletePhotoFunction = dataContext =>
            {
                var photoFromDatabase = dataContext.GetTable<PhotoModel>().Where(x => x.Id.Equals(id)).FirstOrDefault();

                photoFromDatabase.IsDeleted = true;
                photoFromDatabase.UpdatedAt = DateTimeOffset.UtcNow;

                return photoFromDatabase;
            };

            return PerformDatabaseFunction(deletePhotoFunction, _connectionString);
        }
        #endregion
    }
}
