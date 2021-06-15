using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using AzureBlobStorageSampleApp.Shared;

namespace AzureBlobStorageSampleApp
{
    public static class DatabaseSyncService
    {
        public static async Task SyncRemoteAndLocalDatabases()
        {
            var (photoListFromLocalDatabase, photoListFromRemoteDatabase) = await GetAllSavedPhotos().ConfigureAwait(false);

            var (photosInLocalDatabaseButNotStoredRemotely, photosInRemoteDatabaseButNotStoredLocally, photosInBothDatabases) = GetMatchingModels(photoListFromLocalDatabase, photoListFromRemoteDatabase);

            var (photosToPatchToLocalDatabase, photosToPatchToRemoteDatabase) = GetModelsThatNeedUpdating(photoListFromLocalDatabase, photoListFromRemoteDatabase, photosInBothDatabases);

            await SavePhotos(photosInRemoteDatabaseButNotStoredLocally.Concat(photosToPatchToLocalDatabase).ToList(), photosInLocalDatabaseButNotStoredRemotely.Concat(photosToPatchToRemoteDatabase).ToList()).ConfigureAwait(false);
        }

        static async Task<(IReadOnlyList<PhotoModel> photoListFromLocalDatabase,
            IReadOnlyList<PhotoModel> photoListFromRemoteDatabase)> GetAllSavedPhotos()
        {
            var photoListFromLocalDatabaseTask = PhotoDatabase.GetAllPhotos();
            var photoListFromRemoteDatabaseTask = APIService.GetAllPhotoModels();

            await Task.WhenAll(photoListFromLocalDatabaseTask, photoListFromRemoteDatabaseTask).ConfigureAwait(false);

            return (await photoListFromLocalDatabaseTask.ConfigureAwait(false),
                    await photoListFromRemoteDatabaseTask.ConfigureAwait(false));
        }

        static (IReadOnlyList<T> contactsInLocalDatabaseButNotStoredRemotely,
            IReadOnlyList<T> contactsInRemoteDatabaseButNotStoredLocally,
            IReadOnlyList<T> contactsInBothDatabases) GetMatchingModels<T>(IEnumerable<T> modelListFromLocalDatabase,
                                                                      IEnumerable<T> modelListFromRemoteDatabase) where T : IBaseModel
        {
            var modelIdFromRemoteDatabaseList = modelListFromRemoteDatabase?.Select(x => x.Id).ToList() ?? Enumerable.Empty<string>();
            var modelIdFromLocalDatabaseList = modelListFromLocalDatabase?.Select(x => x.Id).ToList() ?? Enumerable.Empty<string>();

            var modelIdsInRemoteDatabaseButNotStoredLocally = modelIdFromRemoteDatabaseList?.Except(modelIdFromLocalDatabaseList)?.ToList() ?? Enumerable.Empty<string>();
            var modelIdsInLocalDatabaseButNotStoredRemotely = modelIdFromLocalDatabaseList?.Except(modelIdFromRemoteDatabaseList)?.ToList() ?? Enumerable.Empty<string>();
            var modelIdsInBothDatabases = modelIdFromRemoteDatabaseList?.Where(x => modelIdFromLocalDatabaseList?.Contains(x) ?? false).ToList() ?? Enumerable.Empty<string>();

            var modelsInRemoteDatabaseButNotStoredLocally = modelListFromRemoteDatabase?.Where(x => modelIdsInRemoteDatabaseButNotStoredLocally?.Contains(x?.Id) ?? false).ToList() ?? Enumerable.Empty<T>();
            var modelsInLocalDatabaseButNotStoredRemotely = modelListFromLocalDatabase?.Where(x => modelIdsInLocalDatabaseButNotStoredRemotely?.Contains(x?.Id) ?? false).ToList() ?? Enumerable.Empty<T>();

            var modelsInBothDatabases = modelListFromLocalDatabase?.Where(x => modelIdsInBothDatabases?.Contains(x?.Id) ?? false)
                                            .ToList() ?? Enumerable.Empty<T>();

            return ((modelsInLocalDatabaseButNotStoredRemotely ?? Enumerable.Empty<T>()).ToList(),
                    (modelsInRemoteDatabaseButNotStoredLocally ?? Enumerable.Empty<T>()).ToList(),
                    (modelsInBothDatabases ?? Enumerable.Empty<T>()).ToList());

        }

        static (IReadOnlyList<T> contactsToPatchToLocalDatabase,
            IReadOnlyList<T> contactsToPatchToRemoteDatabase) GetModelsThatNeedUpdating<T>(IEnumerable<T> modelListFromLocalDatabase,
                                                                              IEnumerable<T> modelListFromRemoteDatabase,
                                                                              IEnumerable<T> modelsFoundInBothDatabases) where T : IBaseModel
        {
            var modelsToPatchToRemoteDatabase = new List<T>();
            var modelsToPatchToLocalDatabase = new List<T>();
            foreach (var contact in modelsFoundInBothDatabases)
            {
                var modelFromLocalDatabase = modelListFromLocalDatabase.Where(x => x.Id.Equals(contact.Id)).FirstOrDefault();
                var modelFromRemoteDatabase = modelListFromRemoteDatabase.Where(x => x.Id.Equals(contact.Id)).FirstOrDefault();

                if (modelFromLocalDatabase?.UpdatedAt.CompareTo(modelFromRemoteDatabase?.UpdatedAt ?? default) > 0)
                    modelsToPatchToRemoteDatabase.Add(modelFromLocalDatabase);
                else if (modelFromRemoteDatabase is not null && modelFromLocalDatabase?.UpdatedAt.CompareTo(modelFromRemoteDatabase.UpdatedAt) < 0)
                    modelsToPatchToLocalDatabase.Add(modelFromRemoteDatabase);
            }

            return (modelsToPatchToLocalDatabase ?? Enumerable.Empty<T>().ToList(),
                    modelsToPatchToRemoteDatabase ?? Enumerable.Empty<T>().ToList());
        }

        static Task SavePhotos(IEnumerable<PhotoModel> photosToSaveToLocalDatabase,
                                IEnumerable<PhotoModel> photosToSaveToRemoteDatabase)
        {
            var savephotoTaskList = new List<Task>();

            foreach (var photo in photosToSaveToLocalDatabase)
                savephotoTaskList.Add(PhotoDatabase.SavePhoto(photo));

            //ToDo Add Patch API

            return Task.WhenAll(savephotoTaskList);
        }
    }
}
