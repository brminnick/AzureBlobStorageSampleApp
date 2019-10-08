using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using AzureBlobStorageSampleApp.Mobile.Shared;
using AzureBlobStorageSampleApp.Shared;

using Polly;
using Refit;

namespace AzureBlobStorageSampleApp
{
    static class APIService
    {
        readonly static Lazy<IPhotosAPI> _photosApiClientHolder = new Lazy<IPhotosAPI>(() => RestService.For<IPhotosAPI>(new HttpClient { BaseAddress = new Uri(BackendConstants.FunctionsAPIBaseUrl) }));

        static IPhotosAPI PhotosApiClient => _photosApiClientHolder.Value;

        public static Task<List<PhotoModel>> GetAllPhotoModels() => ExecutePollyFunction(PhotosApiClient.GetAllPhotoModels);
        public static Task<PhotoModel> PostPhotoBlob(PhotoBlobModel photoBlob, string photoTitle) => ExecutePollyFunction(() => PhotosApiClient.PostPhotoBlob(photoBlob, photoTitle, BackendConstants.PostPhotoBlobFunctionKey));

        static Task<T> ExecutePollyFunction<T>(Func<Task<T>> action, int numRetries = 3)
        {
            return Policy.Handle<Exception>().WaitAndRetryAsync(numRetries, pollyRetryAttempt).ExecuteAsync(action);

            TimeSpan pollyRetryAttempt(int attemptNumber) => TimeSpan.FromSeconds(Math.Pow(2, attemptNumber));
        }
    }
}
