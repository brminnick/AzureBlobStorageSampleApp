using System;
using System.Threading.Tasks;
using AzureBlobStorageSampleApp.Mobile.Shared;

using FFImageLoading.Forms;

using Xamarin.Forms;

namespace AzureBlobStorageSampleApp
{
    public class AddPhotoPage : BaseContentPage<AddPhotoViewModel>
    {
        public AddPhotoPage()
        {
            ViewModel.NoCameraFound += HandleNoCameraFound;
            ViewModel.SavePhotoCompleted += HandleSavePhotoCompleted;
            ViewModel.SavePhotoFailed += HandleSavePhotoFailed;

            var photoTitleEntry = new Entry
            {
                ClearButtonVisibility = ClearButtonVisibility.WhileEditing,
                Placeholder = "Title",
                BackgroundColor = Color.White,
                TextColor = ColorConstants.TextColor,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                ReturnType = ReturnType.Go
            };
            photoTitleEntry.SetBinding(Entry.TextProperty, nameof(AddPhotoViewModel.PhotoTitle));
            photoTitleEntry.SetBinding(Entry.ReturnCommandProperty, nameof(AddPhotoViewModel.TakePhotoCommand));

            var takePhotoButton = new Button
            {
                Text = "Take Photo",
                BackgroundColor = ColorConstants.NavigationBarBackgroundColor,
                TextColor = ColorConstants.TextColor
            };
            takePhotoButton.SetBinding(Button.CommandProperty, nameof(AddPhotoViewModel.TakePhotoCommand));
            takePhotoButton.SetBinding(IsEnabledProperty, new Binding(nameof(AddPhotoViewModel.IsPhotoSaving), BindingMode.Default, new InverseBooleanConverter(), ViewModel.IsPhotoSaving));

            var photoImage = new CachedImage();
            photoImage.SetBinding(CachedImage.SourceProperty, nameof(AddPhotoViewModel.PhotoImageSource));

            var saveToobarItem = new ToolbarItem
            {
                Text = "Save",
                Priority = 0,
                AutomationId = AutomationIdConstants.AddPhotoPage_SaveButton,
            };
            saveToobarItem.SetBinding(MenuItem.CommandProperty, nameof(AddPhotoViewModel.SavePhotoCommand));
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
            activityIndicator.SetBinding(IsVisibleProperty, nameof(AddPhotoViewModel.IsPhotoSaving));
            activityIndicator.SetBinding(ActivityIndicator.IsRunningProperty, nameof(AddPhotoViewModel.IsPhotoSaving));

            this.SetBinding(TitleProperty, nameof(AddPhotoViewModel.PhotoTitle));

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

        void HandleSavePhotoCompleted(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await DisplayAlert("Photo Saved", string.Empty, "OK");
                await ClosePage();
            });
        }

        async void HandleCancelToolbarItemClicked(object sender, EventArgs e)
        {
            if (!ViewModel.IsPhotoSaving)
                await ClosePage();
        }

        async void HandleSavePhotoFailed(object sender, string errorMessage) => await DisplayErrorMessage(errorMessage);

        async void HandleNoCameraFound(object sender, EventArgs e) => await DisplayErrorMessage("No Camera Found");

        Task DisplayErrorMessage(string message) =>
            Device.InvokeOnMainThreadAsync(() => DisplayAlert("Error", message, "Ok"));

        Task ClosePage() => Device.InvokeOnMainThreadAsync(Navigation.PopModalAsync);
    }
}
