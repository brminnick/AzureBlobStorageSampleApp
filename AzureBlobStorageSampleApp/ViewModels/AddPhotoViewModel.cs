using System;
using System.IO;
using System.Windows.Input;
using System.Threading.Tasks;

using Xamarin.Forms;

using Plugin.Media;
using Plugin.Media.Abstractions;

using AzureBlobStorageSampleApp.Shared;
using AzureBlobStorageSampleApp.Mobile.Shared;
using AsyncAwaitBestPractices.MVVM;
using AsyncAwaitBestPractices;

namespace AzureBlobStorageSampleApp
{
    public class AddPhotoViewModel : BaseViewModel
    {
        #region Constant Fields
        readonly WeakEventManager _noCameraFoundEventManager = new WeakEventManager();
        readonly WeakEventManager _savePhotoCompletedEventManager = new WeakEventManager();
        readonly WeakEventManager<string> _savePhotoFailedEventManager = new WeakEventManager<string>();
        #endregion

        #region Fields
        ICommand _savePhotoCommand, _takePhotoCommand;
        string _photoTitle, _pageTitle = PageTitles.AddPhotoPage;
        bool _isPhotoSaving;
        ImageSource _photoImageSource;
        PhotoBlobModel _photoBlob;
        #endregion

        #region Events
        public event EventHandler NoCameraFound
        {
            add => _noCameraFoundEventManager.AddEventHandler(value);
            remove => _noCameraFoundEventManager.RemoveEventHandler(value);
        }

        public event EventHandler SavePhotoCompleted
        {
            add => _savePhotoCompletedEventManager.AddEventHandler(value);
            remove => _savePhotoCompletedEventManager.RemoveEventHandler(value);
        }

        public event EventHandler<string> SavePhotoFailed
        {
            add => _savePhotoFailedEventManager.AddEventHandler(value);
            remove => _savePhotoFailedEventManager.RemoveEventHandler(value);
        }
        #endregion

        #region Properties
        public ICommand TakePhotoCommand => _takePhotoCommand ??
            (_takePhotoCommand = new AsyncCommand(ExecuteTakePhotoCommand, continueOnCapturedContext: false));

        public ICommand SavePhotoCommand => _savePhotoCommand ??
            (_savePhotoCommand = new AsyncCommand(() => ExecuteSavePhotoCommand(PhotoBlob, PhotoTitle), continueOnCapturedContext: false));

        public string PageTitle
        {
            get => _pageTitle;
            set => SetProperty(ref _pageTitle, value);
        }

        public bool IsPhotoSaving
        {
            get => _isPhotoSaving;
            set => SetProperty(ref _isPhotoSaving, value);
        }

        public string PhotoTitle
        {
            get => _photoTitle;
            set => SetProperty(ref _photoTitle, value, UpdatePageTilte);
        }

        public ImageSource PhotoImageSource
        {
            get => _photoImageSource;
            set => SetProperty(ref _photoImageSource, value);
        }

        PhotoBlobModel PhotoBlob
        {
            get => _photoBlob;
            set => SetProperty(ref _photoBlob, value, UpdatePhotoImageSource);
        }
        #endregion

        #region Methods
        async Task ExecuteSavePhotoCommand(PhotoBlobModel photoBlob, string photoTitle)
        {
            if (IsPhotoSaving)
                return;

            if (string.IsNullOrWhiteSpace(BackendConstants.PostPhotoBlobFunctionKey))
            {
                OnSavePhotoFailed("Invalid Azure Function Key");
                return;
            }

            if (string.IsNullOrWhiteSpace(photoTitle))
            {
                OnSavePhotoFailed("Title Cannot Be Empty");
                return;
            }

            IsPhotoSaving = true;

            try
            {
                var photo = await APIService.PostPhotoBlob(photoBlob, photoTitle).ConfigureAwait(false);

                await PhotoDatabase.SavePhoto(photo).ConfigureAwait(false);
                OnSavePhotoCompleted();
            }
            catch (Exception e)
            {
                OnSavePhotoFailed(e.Message);
            }
            finally
            {
                IsPhotoSaving = false;
            }
        }

        async Task ExecuteTakePhotoCommand()
        {
            var mediaFile = await GetMediaFileFromCamera().ConfigureAwait(false);

            if (mediaFile is null)
                return;

            PhotoBlob = new PhotoBlobModel
            {
                Image = ConvertStreamToByteArrary(mediaFile.GetStream())
            };
        }

        byte[] ConvertStreamToByteArrary(Stream stream)
        {
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        async Task<MediaFile> GetMediaFileFromCamera()
        {
            await CrossMedia.Current.Initialize().ConfigureAwait(false);

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                OnNoCameraFound();
                return null;
            }

            var mediaFileTCS = new TaskCompletionSource<MediaFile>();
            Device.BeginInvokeOnMainThread(async () =>
            {
                var photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    PhotoSize = PhotoSize.Small,
                    DefaultCamera = CameraDevice.Rear
                }).ConfigureAwait(false);

                mediaFileTCS.SetResult(photo);
            });

            return await mediaFileTCS.Task;
        }

        void UpdatePageTilte()
        {
            if (string.IsNullOrWhiteSpace(PhotoTitle))
                PageTitle = PageTitles.AddPhotoPage;
            else
                PageTitle = PhotoTitle;
        }

        void UpdatePhotoImageSource() =>
            PhotoImageSource = ImageSource.FromStream(() => new MemoryStream(PhotoBlob.Image));

        void OnSavePhotoFailed(string errorMessage) => _savePhotoFailedEventManager.HandleEvent(this, errorMessage, nameof(SavePhotoFailed));
        void OnNoCameraFound() => _noCameraFoundEventManager.HandleEvent(this, EventArgs.Empty, nameof(NoCameraFound));
        void OnSavePhotoCompleted() => _savePhotoCompletedEventManager.HandleEvent(this, EventArgs.Empty, nameof(SavePhotoCompleted));
        #endregion
    }
}
