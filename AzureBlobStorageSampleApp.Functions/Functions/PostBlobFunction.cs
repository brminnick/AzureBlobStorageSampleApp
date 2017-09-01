using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Extensions.Http;

using AzureBlobStorageSampleApp.Shared;
using AzureBlobStorageSampleApp.Backend.Common;

namespace AzureBlobStorageSampleApp.Functions
{
    public static class PostBlobFunction
    {
        #region Methods
        [FunctionName("PostBlob")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "PostBlob/{title}")]HttpRequestMessage req, string title, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            try
            {
                var imageBlob = await JsonService.DeserializeHttpRequestMessage<PhotoBlob>(req);
                var photo = await BlobStorageService.SavePhoto(imageBlob.Image, title);

                return req.CreateResponse(HttpStatusCode.Created, photo);
            }
            catch (Exception e)
            {
                return req.CreateResponse(HttpStatusCode.InternalServerError, $"Post Blob Failed: {e.Message}");
            }
        }
        #endregion
    }
}
