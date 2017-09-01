using System.Threading.Tasks;
using System.Collections.Generic;

using AzureBlobStorageSampleApp.Shared;

namespace AzureBlobStorageSampleApp
{
    public abstract class PhotoDatabase : BaseDatabase
    {
        #region Methods
        public static async Task SavePhoto(PhotoModel photo)
        {
            var databaseConnection = await GetDatabaseConnectionAsync();

            await databaseConnection.InsertOrReplaceAsync(photo);
        }

        public static async Task<int> GetPhotoCount()
        {
            var databaseConnection = await GetDatabaseConnectionAsync();

            return await databaseConnection.Table<PhotoModel>().CountAsync();
        }

        public static async Task<List<PhotoModel>> GetAllPhotos()
        {
            var databaseConnection = await GetDatabaseConnectionAsync();

            return await databaseConnection.Table<PhotoModel>().ToListAsync();
        }

        public static async Task<PhotoModel> GetPhoto(string id)
        {
            var databaseConnection = await GetDatabaseConnectionAsync();

            return await databaseConnection.Table<PhotoModel>().Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();
        }

        public static async Task DeletePhoto(PhotoModel photo)
        {
            var databaseConnection = await GetDatabaseConnectionAsync();

            photo.IsDeleted = true;

            await databaseConnection.UpdateAsync(photo);
        }

#if DEBUG
        public static async Task RemovePhoto(PhotoModel photo)
        {
            var databaseConnection = await GetDatabaseConnectionAsync();

            await databaseConnection.DeleteAsync(photo);
        }
#endif  
        #endregion
    }
}
