using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AsyncAwaitBestPractices;
using AsyncAwaitBestPractices.MVVM;
using AzureBlobStorageSampleApp.Shared;

namespace AzureBlobStorageSampleApp
{
    public class PhotoListViewModel : BaseViewModel
    {
        readonly WeakEventManager<Exception> _refreshFailedEventManager = new WeakEventManager<Exception>();

        bool _isRefreshing;
        ICommand? _refreshCommand;

        public ObservableCollection<PhotoModel> AllPhotosList { get; } = new ObservableCollection<PhotoModel>();
        public ICommand RefreshCommand => _refreshCommand ??= new AsyncCommand(ExecuteRefreshCommand);

        public event EventHandler<Exception> RefreshFailed
        {
            add => _refreshFailedEventManager.AddEventHandler(value);
            remove => _refreshFailedEventManager.RemoveEventHandler(value);
        }

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

                await DatabaseSyncService.SyncRemoteAndLocalDatabases().ConfigureAwait(false);

                var unsortedPhotosList = await PhotoDatabase.GetAllPhotos().ConfigureAwait(false);

                AllPhotosList.Clear();

                foreach (var photo in unsortedPhotosList.OrderBy(x => x.Title))
                {
                    AllPhotosList.Add(photo);
                }

                await oneSecondTaskToShowSpinner.ConfigureAwait(false);
            }
            catch (Exception e)
            {
                DebugServices.Log(e);
                OnRefreshFailedEventManager(e);
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        void OnRefreshFailedEventManager(Exception exception) => _refreshFailedEventManager.HandleEvent(this, exception, nameof(RefreshFailed));
    }
}
