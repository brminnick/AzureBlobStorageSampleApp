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
            GetDataObjectFromAPI<List<PhotoModel>>($"{BackendConstants.GetAllPhotosUrl}");

        public Task<HttpResponseMessage> PostPhotoBlob(PhotoBlobModel photoBlob, string photoTitle) =>
            PostObjectToAPI($"{BackendConstants.PostPhotoBlobUrl}{photoTitle}{BackendConstants.PostPhotoBlobFunctionKey}", photoBlob);
        #endregion
    }
}
