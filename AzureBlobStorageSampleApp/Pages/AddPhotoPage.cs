using System;

using AzureBlobStorageSampleApp.Mobile.Shared;

using FFImageLoading.Forms;

using Xamarin.Forms;

namespace AzureBlobStorageSampleApp
{
    public class AddPhotoPage : BaseContentPage<AddPhotoViewModel>
    {
        #region Constant Fields
        readonly ToolbarItem _saveToobarItem, _cancelToolbarItem;
        readonly Entry _photoTitleEntry;
        readonly CachedImage _photoImage;
        readonly Button _takePhotoButton;
        #endregion

        #region Constructors
        public AddPhotoPage()
        {
            _photoTitleEntry = new Entry
            {
                Placeholder = "Title",
                BackgroundColor = Color.White,
                TextColor = ColorConstants.TextColor,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                ReturnType = ReturnType.Go
            };
            _photoTitleEntry.SetBinding(Entry.TextProperty, nameof(ViewModel.PhotoTitle));
            _photoTitleEntry.SetBinding(Entry.ReturnCommandProperty, nameof(ViewModel.TakePhotoCommand));

            _takePhotoButton = new Button
            {
                Text = "Take Photo",
                BackgroundColor = ColorConstants.NavigationBarBackgroundColor,
                TextColor = ColorConstants.TextColor
            };
            _takePhotoButton.SetBinding(Button.CommandProperty, nameof(ViewModel.TakePhotoCommand));
            _takePhotoButton.SetBinding(IsEnabledProperty, new Binding(nameof(ViewModel.IsPhotoSaving), BindingMode.Default, new InverseBooleanConverter(), ViewModel.IsPhotoSaving));

            _photoImage = new CachedImage();
            _photoImage.SetBinding(CachedImage.SourceProperty, nameof(ViewModel.PhotoImageSource));

            _saveToobarItem = new ToolbarItem
            {
                Text = "Save",
                Priority = 0,
                AutomationId = AutomationIdConstants.AddPhotoPage_SaveButton,
            };
            _saveToobarItem.SetBinding(MenuItem.CommandProperty, nameof(ViewModel.SavePhotoCommand));
            ToolbarItems.Add(_saveToobarItem);

            _cancelToolbarItem = new ToolbarItem
            {
                Text = "Cancel",
                Priority = 1,
                AutomationId = AutomationIdConstants.CancelButton
            };
            ToolbarItems.Add(_cancelToolbarItem);

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
                    _photoImage,
                    _photoTitleEntry,
                    _takePhotoButton,
                    activityIndicator
                }
            };

            Content = new ScrollView { Content = stackLayout };
        }
        #endregion

        #region Methods
        protected override void SubscribeEventHandlers()
        {
            ViewModel.NoCameraFound += HandleNoCameraFound;
            _cancelToolbarItem.Clicked += HandleCancelToolbarItemClicked;
            ViewModel.SavePhotoCompleted += HandleSavePhotoCompleted;
            ViewModel.SavePhotoFailed += HandleSavePhotoFailed;
        }

        protected override void UnsubscribeEventHandlers()
        {
            ViewModel.NoCameraFound -= HandleNoCameraFound;
            _cancelToolbarItem.Clicked -= HandleCancelToolbarItemClicked;
            ViewModel.SavePhotoCompleted -= HandleSavePhotoCompleted;
            ViewModel.SavePhotoFailed -= HandleSavePhotoFailed;
        }

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
