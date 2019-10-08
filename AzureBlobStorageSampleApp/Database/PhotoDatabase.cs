using System.Threading.Tasks;
using System.Collections.Generic;

using AzureBlobStorageSampleApp.Shared;

namespace AzureBlobStorageSampleApp
{
	public abstract class PhotoDatabase : BaseDatabase
	{
		public static async Task SavePhoto(PhotoModel photo)
		{
			var databaseConnection = await GetDatabaseConnection<PhotoModel>().ConfigureAwait(false);

			await AttemptAndRetry(() => databaseConnection.InsertOrReplaceAsync(photo)).ConfigureAwait(false);
		}

		public static async Task<int> GetPhotoCount()
		{
			var databaseConnection = await GetDatabaseConnection<PhotoModel>().ConfigureAwait(false);

			return await AttemptAndRetry(() => databaseConnection.Table<PhotoModel>().CountAsync()).ConfigureAwait(false);
		}

		public static async Task<List<PhotoModel>> GetAllPhotos()
		{
			var databaseConnection = await GetDatabaseConnection<PhotoModel>().ConfigureAwait(false);

			return await AttemptAndRetry(() => databaseConnection.Table<PhotoModel>().ToListAsync()).ConfigureAwait(false);
		}

		public static async Task<PhotoModel> GetPhoto(string id)
		{
			var databaseConnection = await GetDatabaseConnection<PhotoModel>().ConfigureAwait(false);

			return await AttemptAndRetry(() => databaseConnection.Table<PhotoModel>().Where(x => x.Id.Equals(id)).FirstOrDefaultAsync()).ConfigureAwait(false);
		}

		public static async Task DeletePhoto(PhotoModel photo)
		{
			var databaseConnection = await GetDatabaseConnection<PhotoModel>().ConfigureAwait(false);

			photo.IsDeleted = true;

			await AttemptAndRetry(() => databaseConnection.UpdateAsync(photo)).ConfigureAwait(false);
		}

#if DEBUG
        public static async Task RemovePhoto(PhotoModel photo)
        {
            var databaseConnection = await GetDatabaseConnection<PhotoModel>().ConfigureAwait(false);

            await AttemptAndRetry(() => databaseConnection.DeleteAsync(photo)).ConfigureAwait(false);
        }
#endif
	}
}
