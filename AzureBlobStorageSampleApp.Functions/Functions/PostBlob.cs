using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace AzureBlobStorageSampleApp.Functions
{
    public class PostBlob
    {
        readonly PhotosBlobStorageService _photosBlobStorageService;
        readonly PhotoDatabaseService _photoDatabaseService;

        public PostBlob(PhotosBlobStorageService photosBlobStorageService, PhotoDatabaseService photoDatabaseService) =>
            (_photosBlobStorageService, _photoDatabaseService) = (photosBlobStorageService, photoDatabaseService);

        [FunctionName(nameof(PostBlob))]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "PostBlob/{title}")]HttpRequestMessage req, string title, ILogger log)
        {
            try
            {
                var multipartMemoryStreamProvider = await req.Content.ReadAsMultipartAsync().ConfigureAwait(false);
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
