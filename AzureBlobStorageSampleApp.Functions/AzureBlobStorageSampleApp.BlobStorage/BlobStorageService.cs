using System;
using System.Linq;
using System.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;

using AzureBlobStorageSampleApp.Shared;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AzureBlobStorageSampleApp.Backend.Common
{
    public static class BlobStorageService
    {
        #region Constant Fields
        readonly static Lazy<string> _containerNameHolder = new Lazy<string>(() => ConfigurationManager.AppSettings.GetValues("PhotoContainerName").FirstOrDefault());
        readonly static Lazy<string> _connectionStringHolder = new Lazy<string>(() => ConfigurationManager.ConnectionStrings["BlobStorageConnectionString"].ConnectionString);
        readonly static Lazy<CloudStorageAccount> _storageAccountHolder = new Lazy<CloudStorageAccount>(() => CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting(_connectionStringHolder.Value)));
        readonly static Lazy<CloudBlobClient> _blobClientHolder = new Lazy<CloudBlobClient>(_storageAccountHolder.Value.CreateCloudBlobClient);
        readonly static Lazy<CloudBlobContainer> _blobContainerHolder = new Lazy<CloudBlobContainer>(() => _blobClientHolder.Value.GetContainerReference(ContainerName));
        #endregion

        #region Properties
        static string ContainerName => _containerNameHolder.Value;
        static CloudBlobContainer BlobContainer => _blobContainerHolder.Value;
        #endregion

        #region Methods
        public static async Task<PhotoModel> SavePhoto(byte[] photo, string photoTitle)
        {
            var blockBlob = BlobContainer.GetBlockBlobReference(photoTitle);
            await blockBlob.UploadFromByteArrayAsync(photo, 0, photo.Length);

            return new PhotoModel { Title = photoTitle, Url = blockBlob.Uri.ToString() };
        }

        public static List<PhotoModel> GetAllPhotos()
        {
            var allBlobs = BlobContainer.ListBlobs().ToList();

            var photoList = allBlobs.Where(x => x.Container.Name.Equals(ContainerName)).Select(x => new PhotoModel { Url = x.Uri.ToString() }).ToList();

            return photoList;
        }
        #endregion
    }
}
