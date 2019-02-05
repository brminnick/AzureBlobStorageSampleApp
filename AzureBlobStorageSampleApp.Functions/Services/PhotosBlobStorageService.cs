using System;
using System.Linq;
using System.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.WindowsAzure.Storage.Blob;

using AzureBlobStorageSampleApp.Shared;

namespace AzureBlobStorageSampleApp.Functions
{
    public abstract class PhotosBlobStorageService : BaseBlobStorageService
    {
        #region Constant Fields
        readonly static string _photosContainerName = Environment.GetEnvironmentVariable("PhotoContainerName");
        #endregion

        #region Methods
        public static async Task<PhotoModel> SavePhoto(byte[] photo, string photoTitle)
        {
            var photoBlob = await SaveBlockBlob(_photosContainerName, photo, photoTitle).ConfigureAwait(false);

            return new PhotoModel { Title = photoTitle, Url = photoBlob.Uri.ToString() };
        }

        public static async Task<List<PhotoModel>> GetAllPhotos()
        {
            var blobList = await GetBlobs<CloudBlockBlob>(_photosContainerName).ConfigureAwait(false);

            var photoList = blobList.Select(x => new PhotoModel { Url = x.Uri.ToString() }).ToList();

            return photoList;
        }
        #endregion
    }
}
