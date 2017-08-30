using System;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using Xamarin.Forms;

namespace AzureBlobStorageSampleApp
{
    public class PhotoListViewModel : BaseViewModel
    {
		#region Fields
		bool _isRefreshCommandExecuting;
		ICommand _refreshCommand, _restoreDeletedContactsCommand;
		ObservableCollection<PhotoModel> _allContactsList;
		#endregion

		#region Events
		public event EventHandler PullToRefreshCompleted;
		public event EventHandler RestoreDeletedContactsCompleted;
		#endregion

		#region Properties
		public ICommand RefreshCommand => _refreshCommand ??
			(_refreshCommand = new Command(async () =>await ExecuteRefreshCommand()));

		public ICommand RestoreDeletedContactsCommand => _restoreDeletedContactsCommand ??
			(_restoreDeletedContactsCommand = new Command(async () => await ExecuteRestoreDeletedContactsCommand()));

		public ObservableCollection<PhotoModel> AllPhotosList
		{
			get => _allContactsList;
			set => SetProperty(ref _allContactsList, value);
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

                AllPhotosList = null;

				await oneSecondTaskToShowSpinner;
			}
			catch (Exception e)
			{
				MobileCenterHelpers.Log(e);
			}
			finally
			{
				OnPullToRefreshCompleted();
				_isRefreshCommandExecuting = false;
			}
		}

		async Task ExecuteRestoreDeletedContactsCommand()
		{
			await APIService.RestoreDeletedContacts().ConfigureAwait(false);
			OnRestoreDeletedContactsCompleted();
		}

		void OnPullToRefreshCompleted() =>
			PullToRefreshCompleted?.Invoke(this, EventArgs.Empty);

		void OnRestoreDeletedContactsCompleted() =>
			RestoreDeletedContactsCompleted?.Invoke(this, EventArgs.Empty);
		#endregion
	}
}
