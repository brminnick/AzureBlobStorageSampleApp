using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Extensions.Http;

using AzureBlobStorageSampleApp.Backend.Common;

namespace AzureBlobStorageSampleApp.Functions
{
    public static class GetPhotos
    {
        [FunctionName(nameof(GetPhotos))]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            try
            {
                var photoList = await PhotoDatabaseService.GetAllPhotos();
                return req.CreateResponse(HttpStatusCode.OK, photoList);
            }
            catch (System.Exception e)
            {
                return req.CreateResponse(HttpStatusCode.InternalServerError, $"Get Photos Failed: {e.GetType().ToString()}: {e.Message}");
            }
        }
    }
}
