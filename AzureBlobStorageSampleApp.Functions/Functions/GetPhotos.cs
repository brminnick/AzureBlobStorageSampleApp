using System;
using System.Net;
using System.Net.Http;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;


namespace AzureBlobStorageSampleApp.Functions
{
    public static class GetPhotos
    {
        [FunctionName(nameof(GetPhotos))]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]HttpRequestMessage req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                var photoList = PhotoDatabaseService.GetAllPhotos();
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
