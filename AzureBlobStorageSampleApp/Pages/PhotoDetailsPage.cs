using AzureBlobStorageSampleApp.Shared;
using AzureBlobStorageSampleApp.Mobile.Shared;

using FFImageLoading.Forms;

using Xamarin.Forms;

namespace AzureBlobStorageSampleApp
{
    public class PhotoDetailsPage : BaseContentPage<PhotoDetailsViewModel>
    {
        #region Constructors
        public PhotoDetailsPage(PhotoModel selectedPhoto)
        {
            ViewModel.SetPhotoCommand?.Execute(selectedPhoto);

            var photoTitleLabel = new PhotoDetailLabel(AutomationIdConstants.PhotoTitleLabel);
            photoTitleLabel.SetBinding(Label.TextProperty, nameof(ViewModel.PhotoTitle));

            var photoImage = new CachedImage { AutomationId = AutomationIdConstants.PhotoImage };
            photoImage.SetBinding(CachedImage.SourceProperty, nameof(ViewModel.PhotoImageSource));

            Title = PageTitles.PhotoListPage;

            Padding = new Thickness(20);

            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,

                Spacing = 20,

                Children = {
                    photoImage,
                    photoTitleLabel
                }
            };
        }
        #endregion

        #region Classes
        class PhotoDetailLabel : Label
        {
            public PhotoDetailLabel(string automationId)
            {
                AutomationId = automationId;
                TextColor = Color.FromHex("1B2A38");
                HorizontalTextAlignment = TextAlignment.Center;
                FontAttributes = FontAttributes.Bold;
            }
        }
        #endregion
    }
}
