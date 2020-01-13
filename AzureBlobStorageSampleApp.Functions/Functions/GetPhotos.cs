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
    public class GetPhotos
    {
        readonly PhotoDatabaseService _photoDatabaseService;

        public GetPhotos(PhotoDatabaseService photoDatabaseService) => _photoDatabaseService = photoDatabaseService;

        [FunctionName(nameof(GetPhotos))]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]HttpRequestMessage req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                var photoList = await _photoDatabaseService.GetAllPhotos().ConfigureAwait(false);
                return new OkObjectResult(photoList);
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
