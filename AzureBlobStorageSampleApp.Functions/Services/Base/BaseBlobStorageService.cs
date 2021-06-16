using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Storage.Blob;

namespace AzureBlobStorageSampleApp.Functions
{
    abstract class BaseBlobStorageService
    {
        readonly CloudBlobClient _blobClient;

        protected BaseBlobStorageService(CloudBlobClient cloudBlobClient) => _blobClient = cloudBlobClient;

        protected async Task<CloudBlockBlob> SaveBlockBlob(string containerName, Stream photoStream, string blobTitle)
        {
            var blobContainer = GetBlobContainer(containerName);

            var blockBlob = blobContainer.GetBlockBlobReference(blobTitle);
            await blockBlob.UploadFromStreamAsync(photoStream).ConfigureAwait(false);

            return blockBlob;
        }

        protected async IAsyncEnumerable<T> GetBlobs<T>(string containerName, string prefix = "", int? maxresultsPerQuery = null, BlobListingDetails blobListingDetails = BlobListingDetails.None) where T : ICloudBlob
        {
            var blobContainer = GetBlobContainer(containerName);

            BlobContinuationToken? continuationToken = null;

            do
            {
                var response = await blobContainer.ListBlobsSegmentedAsync(prefix, true, blobListingDetails, maxresultsPerQuery, continuationToken, null, null).ConfigureAwait(false);
                continuationToken = response?.ContinuationToken;

                foreach (var blob in response?.Results?.OfType<T>() ?? Enumerable.Empty<T>())
                {
                    yield return blob;
                }

            } while (continuationToken != null);
        }

        CloudBlobContainer GetBlobContainer(string containerName) => _blobClient.GetContainerReference(containerName);
    }
}
