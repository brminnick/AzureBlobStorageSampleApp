using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

using AzureBlobStorageSampleApp.Shared;
using AzureBlobStorageSampleApp.Backend.Common;

namespace AzureBlobStorageSampleApp.Functions
{
    public static class PostBlob
    {
        #region Methods
        [FunctionName(nameof(PostBlob))]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "PostBlob/{title}")]HttpRequestMessage req, string title, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                var imageBlob = await JsonService.DeserializeMessage<PhotoBlobModel>(req).ConfigureAwait(false);
                var photo = await PhotosBlobStorageService.SavePhoto(imageBlob.Image, title).ConfigureAwait(false);

                await PhotoDatabaseService.InsertPhoto(photo);

                return req.CreateResponse(HttpStatusCode.Created, photo);
            }
            catch (Exception e)
            {
                return req.CreateResponse(HttpStatusCode.InternalServerError, $"Post Blob Failed: {e.GetType().ToString()}: {e.Message}");
            }
        }
        #endregion
    }
}
