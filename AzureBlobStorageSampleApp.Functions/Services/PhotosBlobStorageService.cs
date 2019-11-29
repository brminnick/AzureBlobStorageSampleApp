using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AzureBlobStorageSampleApp.Shared;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AzureBlobStorageSampleApp.Functions
{
    public abstract class PhotosBlobStorageService : BaseBlobStorageService
    {
        readonly static string _photosContainerName = Environment.GetEnvironmentVariable("PhotoContainerName") ?? string.Empty;

        public static async Task<PhotoModel> SavePhoto(Stream photoStream, string photoTitle)
        {
            var photoBlob = await SaveBlockBlob(_photosContainerName, photoStream, photoTitle).ConfigureAwait(false);

            return new PhotoModel { Title = photoTitle, Url = photoBlob.Uri.ToString() };
        }

        public static async Task<List<PhotoModel>> GetAllPhotos()
        {
            var blobList = await GetBlobs<CloudBlockBlob>(_photosContainerName).ConfigureAwait(false);

            var photoList = blobList.Select(x => new PhotoModel { Url = x.Uri.ToString() }).ToList();

            return photoList;
        }
    }
}
