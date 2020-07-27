using System.Threading.Tasks;
using System.Collections.Generic;

using AzureBlobStorageSampleApp.Shared;

namespace AzureBlobStorageSampleApp
{
    public abstract class PhotoDatabase : BaseDatabase
    {
        public static Task SavePhoto(PhotoModel photo) => ExecuteDatabaseFunction<PhotoModel, int>(databaseConnection => databaseConnection.InsertOrReplaceAsync(photo));

        public static Task<int> GetPhotoCount() => ExecuteDatabaseFunction<PhotoModel, int>(databaseConnection => databaseConnection.Table<PhotoModel>().CountAsync());

        public static Task<List<PhotoModel>> GetAllPhotos() => ExecuteDatabaseFunction<PhotoModel, List<PhotoModel>>(databaseConnection => databaseConnection.Table<PhotoModel>().ToListAsync());

        public static Task<PhotoModel> GetPhoto(string id) => ExecuteDatabaseFunction<PhotoModel, PhotoModel>(databaseConnection => databaseConnection.Table<PhotoModel>().Where(x => x.Id.Equals(id)).FirstOrDefaultAsync());

        public static Task DeletePhoto(PhotoModel photo)
        {
            photo.IsDeleted = true;

            return ExecuteDatabaseFunction<PhotoModel, int>(databaseConnection => databaseConnection.UpdateAsync(photo));
        }

#if DEBUG
        public static Task RemovePhoto(PhotoModel photo) => ExecuteDatabaseFunction<PhotoModel, int>(databaseConnection => databaseConnection.DeleteAsync(photo));
#endif
    }
}
