using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AzureBlobStorageSampleApp.Shared;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AzureBlobStorageSampleApp.Functions
{
    public class PhotosBlobStorageService : BaseBlobStorageService
    {
        readonly string _photosContainerName = Environment.GetEnvironmentVariable("PhotoContainerName") ?? string.Empty;

        public async Task<PhotoModel> SavePhoto(Stream photoStream, string photoTitle)
        {
            var photoBlob = await SaveBlockBlob(_photosContainerName, photoStream, photoTitle).ConfigureAwait(false);

            return new PhotoModel { Title = photoTitle, Url = photoBlob.Uri.ToString() };
        }

        public async IAsyncEnumerable<PhotoModel> GetAllPhotos()
        {
            await foreach (var blob in GetBlobs<CloudBlockBlob>(_photosContainerName).ConfigureAwait(false))
            {
                yield return new PhotoModel { Url = blob.Uri.ToString() };
            }
        }
    }
}
