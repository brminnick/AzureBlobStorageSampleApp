using System.Collections.Generic;
using System.Threading.Tasks;

using AzureBlobStorageSampleApp.Shared;

using Refit;

namespace AzureBlobStorageSampleApp
{
    [Headers("Accept-Encoding: gzip",
                "Accept: application/json")]
    public interface IPhotosAPI
    {
        [Get("/GetPhotos")]
        Task<List<PhotoModel>> GetAllPhotoModels();

        [Post("/PostBlob/{photoTitle}")]
        Task<PhotoModel> PostPhotoBlob([Body]PhotoBlobModel photoBlob, string photoTitle, [AliasAs("code")]string functionKey);
    }
}
