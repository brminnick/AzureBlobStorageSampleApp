using Xamarin.Forms;

using AzureBlobStorageSampleApp.Shared;

namespace AzureBlobStorageSampleApp
{
    public class PhotoDetailsViewModel : BaseViewModel
    {
        #region Fields
        Command<PhotoModel> _setPhotoCommand;
        PhotoModel _photo;
        #endregion

        #region Properties
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
        #endregion

        #region Methods
        void NotifyPhotoProperties()
        {
            OnPropertyChanged(nameof(PhotoImageSource));
            OnPropertyChanged(nameof(PhotoTitle));
        }
        #endregion
    }
}
