using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

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
        #region Constant Fields
        static readonly Lazy<JsonSerializer> _serializerHolder = new Lazy<JsonSerializer>();
        #endregion

        #region Properties
        static JsonSerializer Serializer => _serializerHolder.Value;
        #endregion

        #region Methods
        [FunctionName(nameof(PostBlob))]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "PostBlob/{title}")]HttpRequestMessage req, string title, ILogger log)
        {
            try
            {
                var imageBlob = await DeserializeMessage<PhotoBlobModel>(req).ConfigureAwait(false);
                log.LogInformation("Deserialized Image Blob");

                var photo = await PhotosBlobStorageService.SavePhoto(imageBlob.Image, title).ConfigureAwait(false);
                log.LogInformation("Saved Photo to Blob Storage");

                await PhotoDatabaseService.InsertPhoto(photo).ConfigureAwait(false);
                log.LogInformation("Saved Photo to Database");

                return new CreatedResult(photo.Url, photo);
            }
            catch (Exception e)
            {
                log.LogError(e, e.Message);
                return new InternalServerErrorResult();
            }
        }

        static async Task<T> DeserializeMessage<T>(HttpRequestMessage requestMessage)
        {
            using (var stream = await requestMessage.Content.ReadAsStreamAsync().ConfigureAwait(false))
            using (var reader = new StreamReader(stream))
            using (var json = new JsonTextReader(reader))
            {
                if (json is null)
                    return default;

                return await Task.Run(() => Serializer.Deserialize<T>(json)).ConfigureAwait(false);
            }
        }
        #endregion
    }
}
