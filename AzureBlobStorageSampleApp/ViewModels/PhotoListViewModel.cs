using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AsyncAwaitBestPractices;
using AsyncAwaitBestPractices.MVVM;
using AzureBlobStorageSampleApp.Shared;
using Xamarin.Forms;

namespace AzureBlobStorageSampleApp
{
    public class PhotoListViewModel : BaseViewModel
    {
        readonly WeakEventManager<Exception> _refreshFailedEventManager = new WeakEventManager<Exception>();

        bool _isRefreshing;
        ICommand? _refreshCommand;

        public PhotoListViewModel()
        {
            //https://codetraveler.io/2019/09/11/using-observablecollection-in-a-multi-threaded-xamarin-forms-application/
            BindingBase.EnableCollectionSynchronization(AllPhotosList, null, ObservableCollectionCallback);
        }

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
                AllPhotosList.Clear();

                await DatabaseSyncService.SyncRemoteAndLocalDatabases().ConfigureAwait(false);

                var unsortedPhotosList = await PhotoDatabase.GetAllPhotos().ConfigureAwait(false);

                foreach (var photo in unsortedPhotosList.OrderBy(x => x.Title))
                {
                    AllPhotosList.Add(photo);

                    //Pause briefly after each photo is added to allow the UI to show the incoming cascading photos
                    await Task.Delay(100).ConfigureAwait(false);
                }
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

        //https://codetraveler.io/2019/09/11/using-observablecollection-in-a-multi-threaded-xamarin-forms-application/
        void ObservableCollectionCallback(IEnumerable collection, object context, Action accessMethod, bool writeAccess)
        {
            lock(collection)
            {
                accessMethod?.Invoke();
            }
        }

        void OnRefreshFailedEventManager(Exception exception) => _refreshFailedEventManager.HandleEvent(this, exception, nameof(RefreshFailed));
    }
}
