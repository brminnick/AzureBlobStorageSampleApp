using System;
using System.Linq;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using AsyncAwaitBestPractices.MVVM;
using AzureBlobStorageSampleApp.Shared;

namespace AzureBlobStorageSampleApp
{
    public class PhotoListViewModel : BaseViewModel
    {
        bool _isRefreshing;
        ICommand? _refreshCommand;

        public ICommand RefreshCommand => _refreshCommand ??= new AsyncCommand(ExecuteRefreshCommand);
        public ObservableCollection<PhotoModel> AllPhotosList { get; } = new ObservableCollection<PhotoModel>();

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        async Task ExecuteRefreshCommand()
        {
            try
            {
                var oneSecondTaskToShowSpinner = Task.Delay(1000);

                await DatabaseSyncService.SyncRemoteAndLocalDatabases();

                var unsortedPhotosList = await PhotoDatabase.GetAllPhotos();

                AllPhotosList.Clear();

                foreach (var photo in unsortedPhotosList.OrderBy(x => x.Title))
                {
                    AllPhotosList.Add(photo);
                }

                await oneSecondTaskToShowSpinner;
            }
            catch (Exception e)
            {
                DebugServices.Log(e);
            }
            finally
            {
                IsRefreshing = false;
            }
        }
    }
}
