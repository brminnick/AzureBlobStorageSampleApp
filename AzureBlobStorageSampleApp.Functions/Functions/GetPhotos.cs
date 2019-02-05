using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

using AzureBlobStorageSampleApp.Backend.Common;


namespace AzureBlobStorageSampleApp.Functions
{
    public static class GetPhotos
    {
        [FunctionName(nameof(GetPhotos))]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]HttpRequestMessage req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                var photoList = PhotoDatabaseService.GetAllPhotos();
                return req.CreateResponse(HttpStatusCode.OK, photoList);
            }
            catch (System.Exception e)
            {
                return req.CreateResponse(HttpStatusCode.InternalServerError, $"Get Photos Failed: {e.GetType().ToString()}: {e.Message}");
            }
        }
    }
}
