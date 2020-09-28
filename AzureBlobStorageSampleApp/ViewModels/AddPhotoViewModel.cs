using System;
using System.IO;
using System.Threading.Tasks;
using AsyncAwaitBestPractices;
using AsyncAwaitBestPractices.MVVM;
using Xamarin.Essentials;

namespace AzureBlobStorageSampleApp
{
    public class AddPhotoViewModel : BaseViewModel
    {
        readonly WeakEventManager _noCameraFoundEventManager = new WeakEventManager();
        readonly WeakEventManager _savePhotoCompletedEventManager = new WeakEventManager();
        readonly WeakEventManager<string> _savePhotoFailedEventManager = new WeakEventManager<string>();

        FileResult? _photoMediaFile;
        Xamarin.Forms.ImageSource? _photoImageSource;
        AsyncCommand? _savePhotoCommand, _takePhotoCommand;

        string _photoTitle = string.Empty;

        bool _isPhotoSaving;

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

        public AsyncCommand TakePhotoCommand => _takePhotoCommand ??= new AsyncCommand(ExecuteTakePhotoCommand, _ => !IsPhotoSaving);
        public AsyncCommand SavePhotoCommand => _savePhotoCommand ??= new AsyncCommand(() => ExecuteSavePhotoCommand(_photoMediaFile, PhotoTitle),
                                                                                        _ => !IsPhotoSaving && !string.IsNullOrWhiteSpace(PhotoTitle) && PhotoImageSource != null);

        public bool IsPhotoSaving
        {
            get => _isPhotoSaving;
            set => SetProperty(ref _isPhotoSaving, value, async () => await UpdateCanExecute().ConfigureAwait(false));
        }

        public string PhotoTitle
        {
            get => _photoTitle;
            set => SetProperty(ref _photoTitle, value, async () => await UpdateCanExecute().ConfigureAwait(false));
        }

        public Xamarin.Forms.ImageSource? PhotoImageSource
        {
            get => _photoImageSource;
            set => SetProperty(ref _photoImageSource, value, async () => await UpdateCanExecute().ConfigureAwait(false));
        }

        async Task ExecuteSavePhotoCommand(FileResult? photoMediaFile, string photoTitle)
        {
            if (IsPhotoSaving)
                return;

            if (string.IsNullOrWhiteSpace(photoTitle))
            {
                OnSavePhotoFailed("Title Cannot Be Empty");
                return;
            }

            if (photoMediaFile is null)
            {
                OnSavePhotoFailed("Photo Cannot Be Empty");
                return;
            }

            IsPhotoSaving = true;

            try
            {
                var photoModel = await APIService.PostPhotoBlob(photoTitle, photoMediaFile).ConfigureAwait(false);

                await PhotoDatabase.SavePhoto(photoModel).ConfigureAwait(false);

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

            if (mediaFile != null)
            {
                _photoMediaFile = mediaFile;

                var fileStream = await mediaFile.OpenReadAsync().ConfigureAwait(false);
                UpdatePhotoImageSource(fileStream);
            }
        }

        async Task<FileResult?> GetMediaFileFromCamera()
        {
            var arePermissionsGranted = await ArePermissionsGranted().ConfigureAwait(false);

            if (!arePermissionsGranted)
            {
                OnNoCameraFound();
                return null;
            }

            return await MainThread.InvokeOnMainThreadAsync(() => MediaPicker.CapturePhotoAsync()).ConfigureAwait(false);
        }

        Task UpdateCanExecute() => MainThread.InvokeOnMainThreadAsync(() =>
        {
            SavePhotoCommand.RaiseCanExecuteChanged();
            TakePhotoCommand.RaiseCanExecuteChanged();
        });

        Task<bool> ArePermissionsGranted() => MainThread.InvokeOnMainThreadAsync(async () =>
        {
            var cameraStatusRequestTask = Permissions.RequestAsync<Permissions.Camera>();
            var storageWriteStatusRequestTask = Permissions.RequestAsync<Permissions.StorageWrite>();
            var storageReadStatusRequestTask = Permissions.RequestAsync<Permissions.StorageRead>();
            var photosPermissionRequestTask = Permissions.RequestAsync<Permissions.StorageRead>();

            await Task.WhenAll(cameraStatusRequestTask, storageWriteStatusRequestTask, storageReadStatusRequestTask, photosPermissionRequestTask).ConfigureAwait(false);

            var cameraStatus = await cameraStatusRequestTask.ConfigureAwait(false);
            var storageWriteStatus = await storageWriteStatusRequestTask.ConfigureAwait(false);
            var storageReadStatus = await storageReadStatusRequestTask.ConfigureAwait(false);
            var photosPermission = await photosPermissionRequestTask.ConfigureAwait(false);

            return cameraStatus is PermissionStatus.Granted
                    && storageWriteStatus is PermissionStatus.Granted
                    && storageReadStatus is PermissionStatus.Granted
                    && photosPermission is PermissionStatus.Granted;
        });

        void UpdatePhotoImageSource(Stream fileStream) => PhotoImageSource = Xamarin.Forms.ImageSource.FromStream(() => fileStream);

        void OnNoCameraFound() => _noCameraFoundEventManager.RaiseEvent(this, EventArgs.Empty, nameof(NoCameraFound));
        void OnSavePhotoCompleted() => _savePhotoCompletedEventManager.RaiseEvent(this, EventArgs.Empty, nameof(SavePhotoCompleted));
        void OnSavePhotoFailed(in string errorMessage) => _savePhotoFailedEventManager.RaiseEvent(this, errorMessage, nameof(SavePhotoFailed));
    }
}
