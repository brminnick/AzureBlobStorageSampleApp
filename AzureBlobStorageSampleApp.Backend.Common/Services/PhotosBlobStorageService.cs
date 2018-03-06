using System;
using System.Linq;
using System.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.WindowsAzure.Storage.Blob;

using AzureBlobStorageSampleApp.Shared;

namespace AzureBlobStorageSampleApp.Backend.Common
{
    public abstract class PhotosBlobStorageService : BaseBlobStorageService
    {
        #region Constant Fields
        readonly static Lazy<string> _photosContainerNameHolder = new Lazy<string>(ConfigurationManager.AppSettings.GetValues("PhotoContainerName").FirstOrDefault);
        #endregion

        #region Properties
        static string PhotosContainerName => _photosContainerNameHolder.Value;
        #endregion

        #region Methods
        public static async Task<PhotoModel> SavePhoto(byte[] photo, string photoTitle)
        {
            var photoBlob = await SaveBlockBlob(PhotosContainerName, photo, photoTitle).ConfigureAwait(false);

            return new PhotoModel { Title = photoTitle, Url = photoBlob.Uri.ToString() };
        }

        public static async Task<List<PhotoModel>> GetAllPhotos()
        {
            var blobList = await GetBlobs<CloudBlockBlob>(PhotosContainerName).ConfigureAwait(false);

            var photoList = blobList.Select(x => new PhotoModel { Url = x.Uri.ToString() }).ToList();

            return photoList;
        }
        #endregion
    }
}
