using System;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using Xamarin.Forms;

using AzureBlobStorageSampleApp.Shared;

namespace AzureBlobStorageSampleApp
{
    public class PhotoListViewModel : BaseViewModel
    {
        #region Fields
        bool _isRefreshCommandExecuting;
        ICommand _refreshCommand, _restoreDeletedContactsCommand;
		ObservableCollection<PhotoModel> _allPhotosList;
        #endregion

        #region Events
        public event EventHandler PullToRefreshCompleted;
        #endregion

        #region Properties
        public ICommand RefreshCommand => _refreshCommand ??
            (_refreshCommand = new Command(async () => await ExecuteRefreshCommand()));

        public ObservableCollection<PhotoModel> AllPhotosList 
        {
            get => _allPhotosList;
            set => SetProperty(ref _allPhotosList, value);
        }
        #endregion

        #region Methods
        async Task ExecuteRefreshCommand()
        {
            if (_isRefreshCommandExecuting)
                return;

            _isRefreshCommandExecuting = true;
            try
            {
                var oneSecondTaskToShowSpinner = Task.Delay(1000);

                await DatabaseSyncService.SyncRemoteAndLocalDatabases().ConfigureAwait(false);

                AllPhotosList = new ObservableCollection<PhotoModel>(await PhotoDatabase.GetAllPhotos().ConfigureAwait(false));

                await oneSecondTaskToShowSpinner;
            }
            catch (Exception e)
            {
                DebugServices.Log(e);
            }
            finally
            {
                OnPullToRefreshCompleted();
                _isRefreshCommandExecuting = false;
            }
        }

        void OnPullToRefreshCompleted() =>
            PullToRefreshCompleted?.Invoke(this, EventArgs.Empty);
        #endregion
    }
}
