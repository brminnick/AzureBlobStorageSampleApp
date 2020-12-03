using System;
using System.Threading.Tasks;
using AsyncAwaitBestPractices.MVVM;
using AzureBlobStorageSampleApp.Mobile.Shared;
using FFImageLoading.Forms;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.Markup;

namespace AzureBlobStorageSampleApp
{
    public class AddPhotoPage : BaseContentPage<AddPhotoViewModel>
    {
        public AddPhotoPage()
        {
            ViewModel.NoCameraFound += HandleNoCameraFound;
            ViewModel.SavePhotoFailed += HandleSavePhotoFailed;
            ViewModel.SavePhotoCompleted += HandleSavePhotoCompleted;

            this.Bind(TitleProperty, nameof(AddPhotoViewModel.PhotoTitle));

            Title = PageTitles.AddPhotoPage;
            Padding = new Thickness(20);

            Content = new ScrollView
            {
                Content = new StackLayout
                {
                    Spacing = 20,

                    Children =
                    {
                        new CachedImage()
                            .Bind(CachedImage.SourceProperty, nameof(AddPhotoViewModel.PhotoImageSource)),

                        new Entry { ClearButtonVisibility = ClearButtonVisibility.WhileEditing, Placeholder = "Title", BackgroundColor = Color.White, TextColor = ColorConstants.TextColor }.FillExpand()
                            .Bind(Entry.TextProperty, nameof(AddPhotoViewModel.PhotoTitle)),

                        new AddPhotoPageButton("Take Photo")
                            .Bind(Button.CommandProperty,nameof(AddPhotoViewModel.TakePhotoCommand))
                            .Bind<Button, bool, bool>(IsEnabledProperty,nameof(AddPhotoViewModel.IsPhotoSaving), convert: isPhotoSaving => !isPhotoSaving),

                        new ActivityIndicator()
                            .Bind(IsVisibleProperty, nameof(AddPhotoViewModel.IsPhotoSaving))
                            .Bind(ActivityIndicator.IsRunningProperty, nameof(AddPhotoViewModel.IsPhotoSaving))
                    }
                }.Top().FillExpandHorizontal().Assign(out StackLayout stackLayout)
            };

            if (Device.RuntimePlatform is Device.iOS)
            {
                //Add title to UIModalPresentationStyle.FormSheet on iOS
                stackLayout.Children.Insert(0, new Label { Text = PageTitles.AddPhotoPage }.Font(size: 24, bold: true).Margins(0, 24, 5, 0));

                stackLayout.Children.Add(new AddPhotoPageButton("Save")
                                            .Bind<Button, bool, bool>(IsVisibleProperty, nameof(AddPhotoViewModel.IsPhotoSaving), convert: isSaving => !isSaving)
                                            .Bind(Button.CommandProperty, nameof(AddPhotoViewModel.SavePhotoCommand)));
            }
            else
            {
                //Save Button can be added to the Navigation Bar
                ToolbarItems.Add(new ToolbarItem
                {
                    Text = "Save",
                    Priority = 0,
                    AutomationId = AutomationIdConstants.AddPhotoPage_SaveButton,
                }.Bind(MenuItem.CommandProperty, nameof(AddPhotoViewModel.SavePhotoCommand)));

                //Cancel Button only needed for Android becuase iOS can swipe down to return to previous page
                ToolbarItems.Add(new ToolbarItem
                {
                    Text = "Cancel",
                    Priority = 1,
                    AutomationId = AutomationIdConstants.CancelButton,
                    Command = new AsyncCommand(ClosePage, _ => !ViewModel.IsPhotoSaving)
                });
            }
        }

        void HandleSavePhotoCompleted(object sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await DisplayAlert("Photo Saved", string.Empty, "OK");
                await ClosePage();
            });
        }

        async void HandleSavePhotoFailed(object sender, string errorMessage) => await DisplayErrorMessage(errorMessage);

        async void HandleNoCameraFound(object sender, EventArgs e) => await DisplayErrorMessage("No Camera Found");

        Task ClosePage() => MainThread.InvokeOnMainThreadAsync(Navigation.PopModalAsync);

        Task DisplayErrorMessage(string message) =>
            MainThread.InvokeOnMainThreadAsync(() => DisplayAlert("Error", message, "Ok"));

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
