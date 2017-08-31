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
    public static class PhotoDatabaseService
    {
        #region Constant Fields
        readonly static string _connectionString = ConfigurationManager.ConnectionStrings["PhotoDatabaseConnectionString"].ConnectionString;
        #endregion

        #region Methods
        public static async Task<IList<PhotoModel>> GetAllPhotos()
        {
            Func<DataContext, IList<PhotoModel>> getAllPhotosFunction = dataContext => dataContext.GetTable<PhotoModel>().ToList();
            return await PerformDatabaseFunction(getAllPhotosFunction);
        }

        public static async Task<PhotoModel> InsertPhoto(PhotoModel photo)
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

            return await PerformDatabaseFunction(insertPhotoFunction);
        }

        public static async Task<PhotoModel> PatchContactModel(PhotoModel photo)
        {
            var photoModelDelta = new Delta<PhotoModel>();

            photoModelDelta.TrySetPropertyValue(nameof(PhotoModel.Url), photo.Url);
            photoModelDelta.TrySetPropertyValue(nameof(PhotoModel.IsDeleted), photo.IsDeleted);
            photoModelDelta.TrySetPropertyValue(nameof(PhotoModel.Title), photo.Title);

            return await PatchContactModel(photo.Id, photoModelDelta);
        }

        public static async Task<PhotoModel> PatchContactModel(string id, Delta<PhotoModel> photo)
        {
            Func<DataContext, PhotoModel> patchPhotoFunction = dataContext =>
            {
                var photoFromDatabase = dataContext.GetTable<PhotoModel>().Where(x => x.Id.Equals(id)).FirstOrDefault();

                photo.Patch(photoFromDatabase);
                photoFromDatabase.UpdatedAt = DateTimeOffset.UtcNow;

                return photoFromDatabase;
            };

            return await PerformDatabaseFunction(patchPhotoFunction);
        }

        public static async Task<PhotoModel> DeletePhoto(string id)
        {
            Func<DataContext, PhotoModel> deletePhotoFunction = dataContext =>
            {
                var photoFromDatabase = dataContext.GetTable<PhotoModel>().Where(x => x.Id.Equals(id)).FirstOrDefault();

                photoFromDatabase.IsDeleted = true;
                photoFromDatabase.UpdatedAt = DateTimeOffset.UtcNow;

                return photoFromDatabase;
            };

            return await PerformDatabaseFunction(deletePhotoFunction);
        }

        static async Task<TResult> PerformDatabaseFunction<TResult>(Func<DataContext, TResult> databaseFunction) where TResult : class
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();

                var dbContext = new DataContext(sqlConnection);

                var signUpTransaction = sqlConnection.BeginTransaction();
                dbContext.Transaction = signUpTransaction;

                try
                {
                    return databaseFunction?.Invoke(dbContext) ?? default(TResult);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("");
                    Debug.WriteLine(e.Message);
                    Debug.WriteLine(e.ToString());
                    Debug.WriteLine("");

                    return default(TResult);
                }
                finally
                {
                    dbContext.SubmitChanges();
                    signUpTransaction.Commit();
                    sqlConnection.Close();
                }
            }
        }
        #endregion
    }
}
