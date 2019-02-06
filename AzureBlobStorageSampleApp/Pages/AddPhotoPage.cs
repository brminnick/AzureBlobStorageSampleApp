using System;

using AzureBlobStorageSampleApp.Mobile.Shared;

using FFImageLoading.Forms;

using Xamarin.Forms;

namespace AzureBlobStorageSampleApp
{
    public class AddPhotoPage : BaseContentPage<AddPhotoViewModel>
    {
        #region Constructors
        public AddPhotoPage()
        {
            ViewModel.NoCameraFound += HandleNoCameraFound;
            ViewModel.SavePhotoCompleted += HandleSavePhotoCompleted;
            ViewModel.SavePhotoFailed += HandleSavePhotoFailed;

            var photoTitleEntry = new Entry
            {
                Placeholder = "Title",
                BackgroundColor = Color.White,
                TextColor = ColorConstants.TextColor,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                ReturnType = ReturnType.Go
            };
            photoTitleEntry.SetBinding(Entry.TextProperty, nameof(ViewModel.PhotoTitle));
            photoTitleEntry.SetBinding(Entry.ReturnCommandProperty, nameof(ViewModel.TakePhotoCommand));

            var takePhotoButton = new Button
            {
                Text = "Take Photo",
                BackgroundColor = ColorConstants.NavigationBarBackgroundColor,
                TextColor = ColorConstants.TextColor
            };
            takePhotoButton.SetBinding(Button.CommandProperty, nameof(ViewModel.TakePhotoCommand));
            takePhotoButton.SetBinding(IsEnabledProperty, new Binding(nameof(ViewModel.IsPhotoSaving), BindingMode.Default, new InverseBooleanConverter(), ViewModel.IsPhotoSaving));

            var photoImage = new CachedImage();
            photoImage.SetBinding(CachedImage.SourceProperty, nameof(ViewModel.PhotoImageSource));

            var saveToobarItem = new ToolbarItem
            {
                Text = "Save",
                Priority = 0,
                AutomationId = AutomationIdConstants.AddPhotoPage_SaveButton,
            };
            saveToobarItem.SetBinding(MenuItem.CommandProperty, nameof(ViewModel.SavePhotoCommand));
            ToolbarItems.Add(saveToobarItem);

            var cancelToolbarItem = new ToolbarItem
            {
                Text = "Cancel",
                Priority = 1,
                AutomationId = AutomationIdConstants.CancelButton
            };
            cancelToolbarItem.Clicked += HandleCancelToolbarItemClicked;

            ToolbarItems.Add(cancelToolbarItem);

            var activityIndicator = new ActivityIndicator();
            activityIndicator.SetBinding(IsVisibleProperty, nameof(ViewModel.IsPhotoSaving));
            activityIndicator.SetBinding(ActivityIndicator.IsRunningProperty, nameof(ViewModel.IsPhotoSaving));

            this.SetBinding(TitleProperty, nameof(ViewModel.PhotoTitle));

            Padding = new Thickness(20);

            var stackLayout = new StackLayout
            {
                Spacing = 20,

                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.FillAndExpand,

                Children = {
                    photoImage,
                    photoTitleEntry,
                    takePhotoButton,
                    activityIndicator
                }
            };

            Content = new ScrollView { Content = stackLayout };
        }
        #endregion

        #region Methods
        void HandleSavePhotoCompleted(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await DisplayAlert("Photo Saved", string.Empty, "OK");
                ClosePage();
            });
        }

        void HandleCancelToolbarItemClicked(object sender, EventArgs e)
        {
            if (!ViewModel.IsPhotoSaving)
                ClosePage();
        }

        void HandleSavePhotoFailed(object sender, string errorMessage) => DisplayErrorMessage(errorMessage);

        void HandleNoCameraFound(object sender, EventArgs e) => DisplayErrorMessage("No Camera Found");

        void DisplayErrorMessage(string message) =>
            Device.BeginInvokeOnMainThread(async () => await DisplayAlert("Error", message, "Ok"));

        void ClosePage() =>
            Device.BeginInvokeOnMainThread(async () => await Navigation.PopModalAsync());
        #endregion
    }
}
