using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureBlobStorageSampleApp.Functions
{
    class GetPhotos
    {
        readonly PhotoDatabaseService _photoDatabaseService;

        public GetPhotos(PhotoDatabaseService photoDatabaseService) => _photoDatabaseService = photoDatabaseService;

        [Function(nameof(GetPhotos))]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "GET")]HttpRequestData req, FunctionContext context)
        {
            var log = context.GetLogger<GetPhotos>();

            log.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                var photoList = await _photoDatabaseService.GetAllPhotos().ConfigureAwait(false);

                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteStringAsync(JsonConvert.SerializeObject(response)).ConfigureAwait(false);

                return response;
            }
            catch (Exception e)
            {
                log.LogError(e, e.Message);
                return req.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}
