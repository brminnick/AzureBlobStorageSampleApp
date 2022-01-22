using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Newtonsoft.Json;

namespace AzureBlobStorageSampleApp.Functions
{
	abstract class BaseBlobStorageService
	{
		readonly BlobServiceClient _blobServiceClient;

		protected BaseBlobStorageService(BlobServiceClient cloudBlobClient) => _blobServiceClient = cloudBlobClient;

		protected async Task<Azure.Response<BlobContentInfo>> UploadStream(Stream data, string blobName, string containerName)
		{
			var containerClient = GetBlobContainerClient(containerName);
			await containerClient.CreateIfNotExistsAsync().ConfigureAwait(false);

			var blobClient = containerClient.GetBlobClient(blobName);

			return await blobClient.UploadAsync(data).ConfigureAwait(false);
		}

		protected async Task<T> GetLatestValue<T>(string containerName)
		{
			var blobList = new List<BlobItem>();
			await foreach (var blob in GetBlobs(containerName).ConfigureAwait(false))
			{
				blobList.Add(blob);
			}

			var newestBlob = blobList.OrderByDescending(x => x.Properties.CreatedOn).First();

			var blobClient = GetBlobContainerClient(containerName).GetBlobClient(newestBlob.Name);
			var blobContentResponse = await blobClient.DownloadContentAsync().ConfigureAwait(false);

			var serializedBlobContents = blobContentResponse.Value.Content;

			return JsonConvert.DeserializeObject<T>(serializedBlobContents.ToString()) ?? throw new NullReferenceException();
		}

		protected async IAsyncEnumerable<BlobItem> GetBlobs(string containerName)
		{
			var blobContainerClient = GetBlobContainerClient(containerName);

			await foreach (var blob in blobContainerClient.GetBlobsAsync().ConfigureAwait(false))
			{
				if (blob is not null)
					yield return blob;
			}
		}

		protected BlobContainerClient GetBlobContainerClient(string containerName) => _blobServiceClient.GetBlobContainerClient(containerName);
	}
}