using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using AzureBlobStorageSampleApp.Shared;

namespace AzureBlobStorageSampleApp.Functions
{
    class PhotosBlobStorageService : BaseBlobStorageService
    {
        readonly string _photosContainerName = Environment.GetEnvironmentVariable("PhotoContainerName") ?? string.Empty;

        protected PhotosBlobStorageService(BlobServiceClient cloudBlobClient) : base(cloudBlobClient)
        {

        }

        public async Task<PhotoModel> SavePhoto(Stream photoStream, string photoTitle)
        {
            var containerClient = GetBlobContainerClient(_photosContainerName);

            await UploadStream(photoStream, photoTitle, _photosContainerName).ConfigureAwait(false);

            return new PhotoModel { Title = photoTitle, Url = $"{containerClient.Uri}\\{photoTitle}" };
        }

        public async IAsyncEnumerable<PhotoModel> GetAllPhotos()
        {
            var containerClient = GetBlobContainerClient(_photosContainerName);

            await foreach (var blob in GetBlobs(_photosContainerName).ConfigureAwait(false))
            {
                yield return new PhotoModel { Url = $"{containerClient.Uri}\\{blob.Name}" };
            }
        }
    }
}
