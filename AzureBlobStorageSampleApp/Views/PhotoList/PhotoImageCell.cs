using Xamarin.Forms;

using AzureBlobStorageSampleApp.Shared;

namespace AzureBlobStorageSampleApp
{
    public class PhotoImageCell : ImageCell
    {
        public PhotoImageCell()
        {
            var model = BindingContext as PhotoModel;

            this.SetBinding(ImageSourceProperty, nameof(model.Url));
            this.SetBinding(TextProperty, nameof(model.Title));
        }
    }
}
