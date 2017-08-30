using Xamarin.Forms;

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

        public string PhotoUrl
        {
            get => _photo.Url;
            set
            {
                Photo.Url = value;
                NotifyPhotoProperties();
            }
        }

        public string PhotoTitle
        {
            get => _photo.Title;
            set
            {
                Photo.Title = value;
                NotifyPhotoProperties();
            }
        }

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
            OnPropertyChanged(nameof(PhotoUrl));
            OnPropertyChanged(nameof(PhotoTitle));
        }
        #endregion
    }
}
