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
        #region Fields
        bool _isRefreshing;
        ICommand _refreshCommand;
        ObservableCollection<PhotoModel> _allPhotosList;
        #endregion

        #region Properties
        public ICommand RefreshCommand => _refreshCommand ??
            (_refreshCommand = new AsyncCommand(ExecuteRefreshCommand, continueOnCapturedContext: false));

        public ObservableCollection<PhotoModel> AllPhotosList
        {
            get => _allPhotosList;
            set => SetProperty(ref _allPhotosList, value);
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }
        #endregion

        #region Methods
        async Task ExecuteRefreshCommand()
        {
            IsRefreshing = true;

            try
            {
                var oneSecondTaskToShowSpinner = Task.Delay(1000);

                await DatabaseSyncService.SyncRemoteAndLocalDatabases().ConfigureAwait(false);

                var unsortedPhotosList = await PhotoDatabase.GetAllPhotos().ConfigureAwait(false);
                AllPhotosList = new ObservableCollection<PhotoModel>(unsortedPhotosList.OrderBy(x => x.Title));

                await oneSecondTaskToShowSpinner.ConfigureAwait(false);
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
        #endregion
    }
}
