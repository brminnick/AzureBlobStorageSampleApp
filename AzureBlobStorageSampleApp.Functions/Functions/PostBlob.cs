using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using AzureBlobStorageSampleApp.Shared;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

namespace AzureBlobStorageSampleApp.Functions
{
    public static class PostBlob
    {
        static readonly Lazy<JsonSerializer> _serializerHolder = new Lazy<JsonSerializer>();

        static JsonSerializer Serializer => _serializerHolder.Value;

        [FunctionName(nameof(PostBlob))]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "PostBlob/{title}")]HttpRequestMessage req, string title, ILogger log)
        {
            try
            {
                var multipartMemoryStreamProvider = await req.Content.ReadAsMultipartAsync().ConfigureAwait(false);
                var photoStream = await multipartMemoryStreamProvider.Contents[0].ReadAsStreamAsync().ConfigureAwait(false);

                var photo = await PhotosBlobStorageService.SavePhoto(photoStream, title).ConfigureAwait(false);
                log.LogInformation("Saved Photo to Blob Storage");

                await PhotoDatabaseService.InsertPhoto(photo).ConfigureAwait(false);
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

        static async Task<T> DeserializeMessage<T>(HttpRequestMessage requestMessage)
        {
            using var stream = await requestMessage.Content.ReadAsStreamAsync().ConfigureAwait(false);
            using var reader = new StreamReader(stream);
            using var json = new JsonTextReader(reader);

            return await Task.Run(() => Serializer.Deserialize<T>(json)).ConfigureAwait(false);
        }
    }
}
