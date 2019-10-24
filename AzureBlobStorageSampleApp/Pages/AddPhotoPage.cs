using System;
using System.Threading.Tasks;
using AsyncAwaitBestPractices.MVVM;
using AzureBlobStorageSampleApp.Mobile.Shared;
using FFImageLoading.Forms;
using Xamarin.Forms;

namespace AzureBlobStorageSampleApp
{
    public class AddPhotoPage : BaseContentPage<AddPhotoViewModel>
    {
        public AddPhotoPage()
        {
            Disappearing += HandleAddPhotoPageDisappearing;
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
            };
            photoTitleEntry.SetBinding(Entry.TextProperty, nameof(AddPhotoViewModel.PhotoTitle));

            var takePhotoButton = new AddPhotoPageButton("Take Photo");
            takePhotoButton.Clicked += HandleTakePhotoButtonClicked;
            takePhotoButton.SetBinding(IsEnabledProperty, new Binding(nameof(AddPhotoViewModel.IsPhotoSaving), BindingMode.Default, new InverseBooleanConverter(), ViewModel.IsPhotoSaving));

            var photoImage = new CachedImage();
            photoImage.SetBinding(CachedImage.SourceProperty, nameof(AddPhotoViewModel.PhotoImageSource));

            var activityIndicator = new ActivityIndicator();
            activityIndicator.SetBinding(IsVisibleProperty, nameof(AddPhotoViewModel.IsPhotoSaving));
            activityIndicator.SetBinding(ActivityIndicator.IsRunningProperty, nameof(AddPhotoViewModel.IsPhotoSaving));

            this.SetBinding(TitleProperty, nameof(AddPhotoViewModel.PhotoTitle));

            Title = PageTitles.AddPhotoPage;
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
                }
            };

            if (Device.RuntimePlatform is Device.iOS)
            {
                //Add title to UIModalPresentationStyle.FormSheet on iOS
                var titleLabel = new Label
                {
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 24,
                    Margin = new Thickness(0, 24, 5, 0)
                };
                titleLabel.SetBinding(Label.TextProperty, nameof(AddPhotoViewModel.PhotoTitle));
                titleLabel.Text = PageTitles.AddPhotoPage;

                var savePhotoButton = new AddPhotoPageButton("Save");
                savePhotoButton.SetBinding(Button.CommandProperty, nameof(AddPhotoViewModel.SavePhotoCommand));

                stackLayout.Children.Insert(0, titleLabel);
                stackLayout.Children.Add(savePhotoButton);
            }
            else
            {
                //Cancel Button only needed for Android becuase iOS can swipe down to return to previous page
                var cancelToolbarItem = new ToolbarItem
                {
                    Text = "Cancel",
                    Priority = 1,
                    AutomationId = AutomationIdConstants.CancelButton
                };
                cancelToolbarItem.Command = new AsyncCommand(ExecuteCancelButtonCommand);

                //Save Button can be added to the Navigation Bar
                var saveToobarItem = new ToolbarItem
                {
                    Text = "Save",
                    Priority = 0,
                    AutomationId = AutomationIdConstants.AddPhotoPage_SaveButton,
                };
                saveToobarItem.SetBinding(MenuItem.CommandProperty, nameof(AddPhotoViewModel.SavePhotoCommand));

                ToolbarItems.Add(saveToobarItem);
                ToolbarItems.Add(cancelToolbarItem);
            }

            stackLayout.Children.Add(activityIndicator);

            Content = new ScrollView { Content = stackLayout };
        }

        async void HandleTakePhotoButtonClicked(object sender, EventArgs e)
        {
            if (ViewModel.TakePhotoCommand.CanExecute(null))
            {
                Disappearing -= HandleAddPhotoPageDisappearing;

                await ViewModel.TakePhotoCommand.ExecuteAsync();

                Disappearing += HandleAddPhotoPageDisappearing;
            }
        }

        async void HandleSavePhotoCompleted(object sender, EventArgs e)
        {
            await DisplayAlert("Photo Saved", string.Empty, "OK");
            await ClosePage();
        }

        async void HandleAddPhotoPageDisappearing(object sender, EventArgs e)
        {
            if (Navigation.ModalStack.Count > 0)
                await Navigation.PopModalAsync();
        }

        async void HandleSavePhotoFailed(object sender, string errorMessage) => await DisplayErrorMessage(errorMessage);

        async void HandleNoCameraFound(object sender, EventArgs e) => await DisplayErrorMessage("No Camera Found");

        Task ClosePage() => Device.InvokeOnMainThreadAsync(Navigation.PopModalAsync);

        Task DisplayErrorMessage(string message) =>
            Device.InvokeOnMainThreadAsync(() => DisplayAlert("Error", message, "Ok"));

        async Task ExecuteCancelButtonCommand()
        {
            if (!ViewModel.IsPhotoSaving)
                await ClosePage();
        }

        class AddPhotoPageButton : Button
        {
            public AddPhotoPageButton(string text)
            {
                Text = text;
                BackgroundColor = ColorConstants.NavigationBarBackgroundColor;
                TextColor = Color.White;
            }
        }
    }
}
