using Xamarin.Forms;

using AzureBlobStorageSampleApp.Shared;

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

            var photoImage = new PhotoDetailLabel(AutomationIdConstants.PhotoImage);
            photoImage.SetBinding(Image.SourceProperty, nameof(ViewModel.PhotoUrl));

            Title = PageTitles.PhotoListPage;

            Padding = new Thickness(20, 0, 20, 0);

            Content = new StackLayout
            {
                Margin = new Thickness(0, 10, 0, 0),
                Children = {
                    photoTitleLabel,
                    photoImage,
                }
            };
        }
        #endregion

        #region Methods
        protected override void SubscribeEventHandlers()
        {

        }

        protected override void UnsubscribeEventHandlers()
        {

        }
		#endregion

		#region Classes
		class PhotoDetailLabel : Label
		{
            public PhotoDetailLabel(string automationId)
            {
                AutomationId = automationId;
                TextColor = Color.FromHex("1B2A38");
            }
		}
		#endregion
	}
}
