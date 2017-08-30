using System;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;

namespace AzureBlobStorageSampleApp
{
    public class AddPhotoViewModel : BaseViewModel
    {
        #region Fields
        ICommand _savePhotoCommand;
        string _photoTitle;
        ImageSource _photoImageSource;
        #endregion

        #region Properties
        public ICommand SavePhotoCommand => _savePhotoCommand ??
            (_savePhotoCommand = new Command(async () => await ExecuteSavePhotoCommand()));

        public string PhotoTitle
        {
            get => _photoTitle;
            set => SetProperty(ref _photoTitle, value);
        }

        public ImageSource PhotoImageSource 
        {
            get => _photoImageSource;
            set => SetProperty(ref _photoImageSource, value);
        }
        #endregion

        #region Methods
        async Task ExecuteSavePhotoCommand()
        {
            var currentDateTimeOffset = DateTimeOffset.UtcNow;

            var photo = new PhotoModel
            {
                CreatedAt = currentDateTimeOffset,
                UpdatedAt = currentDateTimeOffset,
                Title = PhotoTitle,
                Url = PhotoUrl
            }
        }
        #endregion
    }
}
