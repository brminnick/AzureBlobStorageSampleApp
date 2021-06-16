using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

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
                req.bo

                var content = (HttpContent)new StreamContent(req.Body);
                content.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse(req.Headers.GetValues("Content-Type").Single();

                var multipartMemoryStreamProvider = await req.Body.ReadAsMultipartAsync().ConfigureAwait(false);
                var photoStream = await multipartMemoryStreamProvider.Contents[0].ReadAsStreamAsync().ConfigureAwait(false);

                var photo = await _photosBlobStorageService.SavePhoto(photoStream, title).ConfigureAwait(false);
                log.LogInformation("Saved Photo to Blob Storage");

                await _photoDatabaseService.InsertPhoto(photo).ConfigureAwait(false);
                log.LogInformation("Saved Photo to Database");

                return new CreatedResult(photo.Url, photo);
            }
            catch (Exception e)
            {
                log.LogError(e, e.Message);

                return new ObjectResult(e)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }
    }
}
