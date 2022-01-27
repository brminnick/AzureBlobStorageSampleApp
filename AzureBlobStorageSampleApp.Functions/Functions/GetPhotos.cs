

using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace AzureBlobStorageSampleApp.Functions
{
    class GetPhotos
    {
        readonly PhotoDatabaseService _photoDatabaseService;

        public GetPhotos(PhotoDatabaseService photoDatabaseService) => _photoDatabaseService = photoDatabaseService;

        [Function(nameof(GetPhotos))]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequestData req, FunctionContext context)
        {
            var log = context.GetLogger<GetPhotos>();
            log.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                var photoList = await _photoDatabaseService.GetAllPhotos().ConfigureAwait(false);

                var okResponse = req.CreateResponse(System.Net.HttpStatusCode.OK);
                await okResponse.WriteAsJsonAsync(photoList);

                return okResponse;
            }
            catch (Exception e)
            {
                log.LogError(e, e.Message);

                var errorResponse = req.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
                return errorResponse;
            }
        }
    }
}
