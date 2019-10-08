using Xamarin.Forms;

using AzureBlobStorageSampleApp.Shared;

namespace AzureBlobStorageSampleApp
{
    public class PhotoDetailsViewModel : BaseViewModel
    {
        Command<PhotoModel> _setPhotoCommand;
        PhotoModel _photo;

        public Command<PhotoModel> SetPhotoCommand => _setPhotoCommand ??
            (_setPhotoCommand = new Command<PhotoModel>(photo => Photo = photo));
            
        public ImageSource PhotoImageSource => ImageSource.FromUri(new System.Uri(Photo.Url));
        public string PhotoTitle => Photo.Title;

        PhotoModel Photo
        {
            get => _photo;
            set
            {
                _photo = value;
                NotifyPhotoProperties();
            }
        }

        void NotifyPhotoProperties()
        {
            OnPropertyChanged(nameof(PhotoImageSource));
            OnPropertyChanged(nameof(PhotoTitle));
        }
    }
}
