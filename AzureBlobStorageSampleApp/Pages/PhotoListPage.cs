using System;
using System.Linq;
using AzureBlobStorageSampleApp.Mobile.Shared;
using AzureBlobStorageSampleApp.Shared;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.Markup;

namespace AzureBlobStorageSampleApp
{
    public class PhotoListPage : BaseContentPage<PhotoListViewModel>
    {
        public PhotoListPage()
        {
            ViewModel.RefreshFailed += HandleRefreshFailed;
            Title = PageTitles.PhotoListPage;

            ToolbarItems.Add(new ToolbarItem
            {
                Text = "+",
                AutomationId = AutomationIdConstants.AddPhotoButton
            }.Invoke(addPhotosButton => addPhotosButton.Clicked += HandleAddContactButtonClicked));

            Content = new RefreshView
            {
                Content = new CollectionView
                {
                    ItemTemplate = new PhotoDataTemplate(),
                    SelectionMode = SelectionMode.Single,
                    AutomationId = AutomationIdConstants.PhotosCollectionView,
                }.Bind(CollectionView.ItemsSourceProperty, nameof(PhotoListViewModel.AllPhotosList))
                 .Invoke(collectionView => collectionView.SelectionChanged += HandlePhotoCollectionSelectionChanged)

            }.Bind(RefreshView.IsRefreshingProperty, nameof(PhotoListViewModel.IsRefreshing))
             .Bind(RefreshView.CommandProperty, nameof(PhotoListViewModel.RefreshCommand));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var refreshView = (RefreshView)Content;
            refreshView.IsRefreshing = true;
        }

        async void HandlePhotoCollectionSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var collectionView = (CollectionView)sender;
            collectionView.SelectedItem = null;

            if (e.CurrentSelection.FirstOrDefault() is PhotoModel selectedPhoto)
            {
                await Navigation.PushAsync(new PhotoDetailsPage(selectedPhoto));
            }
        }

        async void HandleAddContactButtonClicked(object sender, EventArgs e)
        {
            //iOS uses UIModalPresentationStyle.FormSheet
            if (Device.RuntimePlatform is Device.iOS)
                await Navigation.PushModalAsync(new AddPhotoPage());
            else
                await Navigation.PushModalAsync(new BaseNavigationPage(new AddPhotoPage()));
        }

        async void HandleRefreshFailed(object sender, Exception exception) =>
            await DisplayAlert("Get Photos Failed", exception.ToString(), "OK");
    }
}
