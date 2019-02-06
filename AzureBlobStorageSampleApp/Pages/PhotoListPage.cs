using System;

using Xamarin.Forms;

using AzureBlobStorageSampleApp.Shared;
using AzureBlobStorageSampleApp.Mobile.Shared;

namespace AzureBlobStorageSampleApp
{
    public class PhotoListPage : BaseContentPage<PhotoListViewModel>
    {
        #region Constant Fields
        readonly ListView _photosListView;
        #endregion

        #region Constructors
        public PhotoListPage()
        {
            var addPhotosButton = new ToolbarItem
            {
                Text = "+",
                AutomationId = AutomationIdConstants.AddPhotoButton
            };
            addPhotosButton.Clicked += HandleAddContactButtonClicked;

            ToolbarItems.Add(addPhotosButton);

            _photosListView = new ListView(ListViewCachingStrategy.RecycleElement)
            {
                ItemTemplate = new DataTemplate(typeof(PhotoViewCell)),
                IsPullToRefreshEnabled = true,
                BackgroundColor = Color.Transparent,
                AutomationId = AutomationIdConstants.PhotoListView,
                SeparatorVisibility = SeparatorVisibility.None
            };
            _photosListView.ItemTapped += HandleItemTapped;
            _photosListView.SetBinding(ListView.IsRefreshingProperty, nameof(ViewModel.IsRefreshing));
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

        void HandleItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (sender is ListView listView && e?.Item is PhotoModel selectedPhoto)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PushAsync(new PhotoDetailsPage(selectedPhoto));
                    listView.SelectedItem = null;
                });
            }
        }

        void HandleAddContactButtonClicked(object sender, EventArgs e) =>
            Device.BeginInvokeOnMainThread(async () => await Navigation.PushModalAsync(new BaseNavigationPage(new AddPhotoPage())));
        #endregion
    }
}
