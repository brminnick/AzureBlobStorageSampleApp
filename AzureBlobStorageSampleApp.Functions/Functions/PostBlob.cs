using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

using AzureBlobStorageSampleApp.Shared;

namespace AzureBlobStorageSampleApp.Functions
{
    public static class PostBlob
    {
        #region Methods
        [FunctionName(nameof(PostBlob))]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "PostBlob/{title}")][FromBody] PhotoBlobModel imageBlob, string title, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                var photo = await PhotosBlobStorageService.SavePhoto(imageBlob.Image, title).ConfigureAwait(false);

                await PhotoDatabaseService.InsertPhoto(photo).ConfigureAwait(false);

                return new CreatedResult(photo.Url, photo);
            }
            catch
            {
                return new InternalServerErrorResult();
            }
        }
        #endregion
    }
}
