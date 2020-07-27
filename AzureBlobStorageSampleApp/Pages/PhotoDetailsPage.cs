using AzureBlobStorageSampleApp.Mobile.Shared;
using AzureBlobStorageSampleApp.Shared;
using FFImageLoading.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Markup;

namespace AzureBlobStorageSampleApp
{
    public class PhotoDetailsPage : BaseContentPage<PhotoDetailsViewModel>
    {
        public PhotoDetailsPage(PhotoModel selectedPhoto)
        {
            ViewModel.SetPhotoCommand?.Execute(selectedPhoto);

            Title = PageTitles.PhotoDetailsPage;

            Padding = new Thickness(20);

            Content = new StackLayout
            {
                Spacing = 20,

                Children =
                {
                    new CachedImage { AutomationId = AutomationIdConstants.PhotoImage }
                        .Bind(CachedImage.SourceProperty, nameof(PhotoDetailsViewModel.PhotoImageSource)),
                    new PhotoDetailLabel(AutomationIdConstants.PhotoTitleLabel)
                        .Bind(Label.TextProperty, nameof(PhotoDetailsViewModel.PhotoTitle))
                }
            }.Center();
        }

        class PhotoDetailLabel : Label
        {
            public PhotoDetailLabel(in string automationId)
            {
                AutomationId = automationId;
                TextColor = Color.FromHex("1B2A38");
                HorizontalTextAlignment = TextAlignment.Center;
                FontAttributes = FontAttributes.Bold;
            }
        }
    }
}
