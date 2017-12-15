using Xamarin.Forms;

using AzureBlobStorageSampleApp.Shared;

namespace AzureBlobStorageSampleApp
{
    public class PhotoImageCell : ImageCell
    {
        public PhotoImageCell()
        {
            this.SetBinding(ImageSourceProperty, nameof(PhotoModel.Url));
            this.SetBinding(TextProperty, nameof(PhotoModel.Title));
        }
    }
}
