using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

using AzureBlobStorageSampleApp.Shared;
using AzureBlobStorageSampleApp.Mobile.Shared;

namespace AzureBlobStorageSampleApp
{
    abstract class APIService : BaseHttpClientService
    {
        #region Methods
        public static Task<List<PhotoModel>> GetAllPhotoModels() =>
            GetObjectFromAPI<List<PhotoModel>>($"{BackendConstants.GetAllPhotosUrl}");

        public static Task<PhotoModel> PostPhotoBlob(PhotoBlobModel photoBlob, string photoTitle) =>
            PostObjectToAPI<PhotoModel, PhotoBlobModel>($"{BackendConstants.PostPhotoBlobUrl}/{photoTitle}?code={BackendConstants.PostPhotoBlobFunctionKey}", photoBlob);
        #endregion
    }
}
