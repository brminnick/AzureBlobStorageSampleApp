using System;
using System.Net;
using System.Threading.Tasks;
using HttpMultipartParser;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace AzureBlobStorageSampleApp.Functions
{
    class PostBlob
    {
        readonly PhotosBlobStorageService _photosBlobStorageService;
        readonly PhotoDatabaseService _photoDatabaseService;

        public PostBlob(PhotosBlobStorageService photosBlobStorageService, PhotoDatabaseService photoDatabaseService) =>
            (_photosBlobStorageService, _photoDatabaseService) = (photosBlobStorageService, photoDatabaseService);

        [Function(nameof(PostBlob))]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "PostBlob/{title}")]HttpRequestData req, string title, FunctionContext context)
        {
            var log = context.GetLogger<PostBlob>();

            try
            {
                var multipartFormParser = await MultipartFormDataParser.ParseAsync(req.Body);
                var photoStream = multipartFormParser.Files[0].Data;

                var photo = await _photosBlobStorageService.SavePhoto(photoStream, title).ConfigureAwait(false);
                log.LogInformation("Saved Photo to Blob Storage");

                await _photoDatabaseService.InsertPhoto(photo).ConfigureAwait(false);
                log.LogInformation("Saved Photo to Database");

                var response = req.CreateResponse(HttpStatusCode.Created);
                await response.WriteAsJsonAsync(photo).ConfigureAwait(false);

                return response;
            }
            catch (Exception e)
            {
                log.LogError(e, e.Message);

                var response = req.CreateResponse(HttpStatusCode.InternalServerError);
                return response;
            }
        }
    }
}
