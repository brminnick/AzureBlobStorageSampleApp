using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureBlobStorageSampleApp.Functions
{
    class PostBlob
    {
        readonly PhotoDatabaseService _photoDatabaseService;
        readonly PhotosBlobStorageService _photosBlobStorageService;

        public PostBlob(PhotosBlobStorageService photosBlobStorageService, PhotoDatabaseService photoDatabaseService) =>
            (_photosBlobStorageService, _photoDatabaseService) = (photosBlobStorageService, photoDatabaseService);

        [Function(nameof(PostBlob))]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "PostBlob/{title}")]HttpRequestData req, string title, FunctionContext context)
        {
            var log = context.GetLogger<PostBlob>();

            try
            {
                var photo = await _photosBlobStorageService.SavePhoto(req.Body, title).ConfigureAwait(false);
                log.LogInformation("Saved Photo to Blob Storage");

                await _photoDatabaseService.InsertPhoto(photo).ConfigureAwait(false);
                log.LogInformation("Saved Photo to Database");

                var response = req.CreateResponse();
                await response.WriteAsJsonAsync(JsonConvert.SerializeObject(photo.Url)).ConfigureAwait(false);

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
