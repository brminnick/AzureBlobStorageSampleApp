using System;

using Xamarin.Forms;

using AzureBlobStorageSampleApp.Shared;
namespace AzureBlobStorageSampleApp
{
    public class PhotoListPage : BaseContentPage<PhotoListViewModel>
    {
        #region Constant Fields
        readonly ListView _photosListView;
        readonly ToolbarItem _addPhotosButton;
        #endregion

        #region Constructors
        public PhotoListPage()
        {
            _addPhotosButton = new ToolbarItem
            {
                Text = "+",
                AutomationId = AutomationIdConstants.AddPhotoButton
            };
            ToolbarItems.Add(_addPhotosButton);

            _photosListView = new ListView(ListViewCachingStrategy.RecycleElement)
            {
                ItemTemplate = new DataTemplate(typeof(PhotoImageCell)),
                IsPullToRefreshEnabled = true,
                BackgroundColor = Color.Transparent,
                AutomationId = AutomationIdConstants.PhotoListView
            };
            _photosListView.SetBinding(ListView.ItemsSourceProperty, nameof(ViewModel.AllPhotosList));
            _photosListView.SetBinding(ListView.RefreshCommandProperty, nameof(ViewModel.RefreshCommand));

            Title = PageTitles.PhotoListPage;

            var relativeLayout = new RelativeLayout();
            relativeLayout.Children.Add(_photosListView,
                                       Constraint.Constant(0),
                                       Constraint.Constant(0),
                                       Constraint.RelativeToParent(parent => parent.Width),
                                       Constraint.RelativeToParent(parent => parent.Height));

            Content = relativeLayout;
        }
        #endregion

        #region Methods
        protected override void OnAppearing()
        {
            base.OnAppearing();

            Device.BeginInvokeOnMainThread(_photosListView.BeginRefresh);
        }

        protected override void SubscribeEventHandlers()
        {
			_photosListView.ItemSelected += HandleItemSelected;
			_addPhotosButton.Clicked += HandleAddContactButtonClicked;
			ViewModel.PullToRefreshCompleted += HandlePullToRefreshCompleted;
			ViewModel.RestoreDeletedContactsCompleted += HandleRestoreDeletedContactsCompleted;
        }

        protected override void UnsubscribeEventHandlers()
        {
			_photosListView.ItemSelected -= HandleItemSelected;
			_addPhotosButton.Clicked -= HandleAddContactButtonClicked;
			ViewModel.PullToRefreshCompleted -= HandlePullToRefreshCompleted;
			ViewModel.RestoreDeletedContactsCompleted -= HandleRestoreDeletedContactsCompleted;
        }

		void HandleItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			var listView = sender as ListView;
			var selectedContactModel = e?.SelectedItem as PhotoModel;

			Device.BeginInvokeOnMainThread(async () =>
			{
				await Navigation.PushAsync(new PhotoDetailsPage(selectedContactModel, false));
				listView.SelectedItem = null;
			});
		}

		void HandleAddContactButtonClicked(object sender, EventArgs e)
		{
			MobileCenterHelpers.TrackEvent(MobileCenterConstants.AddContactButtonTapped);

			Device.BeginInvokeOnMainThread(async () =>
			   await Navigation.PushModalAsync(new BaseNavigationPage(new ContactDetailPage(new ContactModel(), true))));
		}
        #endregion
    }
}
