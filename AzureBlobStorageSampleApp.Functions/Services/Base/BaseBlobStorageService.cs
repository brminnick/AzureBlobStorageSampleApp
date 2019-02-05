using System;
using System.Linq;
using System.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AzureBlobStorageSampleApp.Functions
{
    public abstract class BaseBlobStorageService
    {
        #region Constant Fields
        readonly static Lazy<string> _connectionStringHolder = new Lazy<string>(() => Environment.GetEnvironmentVariable("BlobStorageConnectionString"));
        readonly static Lazy<CloudStorageAccount> _storageAccountHolder = new Lazy<CloudStorageAccount>(() => CloudStorageAccount.Parse(_connectionStringHolder.Value));
        readonly static Lazy<CloudBlobClient> _blobClientHolder = new Lazy<CloudBlobClient>(_storageAccountHolder.Value.CreateCloudBlobClient);
        #endregion

        #region Properties
        static CloudBlobClient BlobClient => _blobClientHolder.Value;
        #endregion

        #region Methods
        protected static async Task<CloudBlockBlob> SaveBlockBlob(string containerName, byte[] blob, string blobTitle)
        {
            var blobContainer = GetBlobContainer(containerName);

            var blockBlob = blobContainer.GetBlockBlobReference(blobTitle);
            await blockBlob.UploadFromByteArrayAsync(blob, 0, blob.Length).ConfigureAwait(false);

            return blockBlob;
        }

        protected static async Task<List<T>> GetBlobs<T>(string containerName, string prefix = "", int? maxresultsPerQuery = null, BlobListingDetails blobListingDetails = BlobListingDetails.None) where T : ICloudBlob
        {
            var blobContainer = GetBlobContainer(containerName);

            var blobList = new List<T>();
            BlobContinuationToken continuationToken = null;

            try
            {
                do
                {
                    var response = await blobContainer.ListBlobsSegmentedAsync(prefix, true, blobListingDetails, maxresultsPerQuery, continuationToken, null, null).ConfigureAwait(false);
                    continuationToken = response?.ContinuationToken;

                    foreach (var blob in response?.Results?.OfType<T>())
                    {
                        blobList.Add(blob);
                    }

                } while (continuationToken != null);
            }
            catch
            {

            }

            return blobList;
        }

        static CloudBlobContainer GetBlobContainer(string containerName) => BlobClient.GetContainerReference(containerName);
        #endregion
    }
}
