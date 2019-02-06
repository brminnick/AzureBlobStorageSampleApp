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
            var databaseConnection = await GetDatabaseConnection().ConfigureAwait(false);

            await databaseConnection.InsertOrReplaceAsync(photo).ConfigureAwait(false);
        }

        public static async Task<int> GetPhotoCount()
        {
            var databaseConnection = await GetDatabaseConnection().ConfigureAwait(false);

            return await databaseConnection.Table<PhotoModel>().CountAsync().ConfigureAwait(false);
        }

        public static async Task<List<PhotoModel>> GetAllPhotos()
        {
            var databaseConnection = await GetDatabaseConnection().ConfigureAwait(false);

            return await databaseConnection.Table<PhotoModel>().ToListAsync().ConfigureAwait(false);
        }

        public static async Task<PhotoModel> GetPhoto(string id)
        {
            var databaseConnection = await GetDatabaseConnection().ConfigureAwait(false);

            return await databaseConnection.Table<PhotoModel>().Where(x => x.Id.Equals(id)).FirstOrDefaultAsync().ConfigureAwait(false);
        }

        public static async Task DeletePhoto(PhotoModel photo)
        {
            var databaseConnection = await GetDatabaseConnection().ConfigureAwait(false);

            photo.IsDeleted = true;

            await databaseConnection.UpdateAsync(photo).ConfigureAwait(false);
        }

#if DEBUG
        public static async Task RemovePhoto(PhotoModel photo)
        {
            var databaseConnection = await GetDatabaseConnection();

            await databaseConnection.DeleteAsync(photo).ConfigureAwait(false);
        }
#endif  
        #endregion
    }
}
