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

        [Post("/PostBlob/{photoTitle}"), Multipart]
        Task<PhotoModel> PostPhotoBlob(string photoTitle, [AliasAs("photo")] StreamPart photoStream);
    }
}
