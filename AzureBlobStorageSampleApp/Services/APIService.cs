using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AzureBlobStorageSampleApp.Mobile.Shared;
using AzureBlobStorageSampleApp.Shared;
using Polly;
using Refit;
using Xamarin.Essentials;

namespace AzureBlobStorageSampleApp
{
    static class APIService
    {
        readonly static Lazy<IPhotosAPI> _photosApiClientHolder = new Lazy<IPhotosAPI>(() => RestService.For<IPhotosAPI>(new HttpClient { BaseAddress = new Uri(BackendConstants.FunctionsAPIBaseUrl) }));

        static IPhotosAPI PhotosApiClient => _photosApiClientHolder.Value;

        public static Task<List<PhotoModel>> GetAllPhotoModels() => ExecutePollyFunction(PhotosApiClient.GetAllPhotoModels);

        public static async Task<PhotoModel> PostPhotoBlob(string photoTitle, FileResult photoMediaFile)
        {
            var fileStream = await photoMediaFile.OpenReadAsync().ConfigureAwait(false);
            return await ExecutePollyFunction(() => PhotosApiClient.PostPhotoBlob(photoTitle, new StreamPart(fileStream, $"{photoTitle}"))).ConfigureAwait(false);
        }

        static Task<T> ExecutePollyFunction<T>(Func<Task<T>> action, int numRetries = 3)
        {
            return Policy.Handle<Exception>().WaitAndRetryAsync(numRetries, pollyRetryAttempt).ExecuteAsync(action);

            static TimeSpan pollyRetryAttempt(int attemptNumber) => TimeSpan.FromSeconds(Math.Pow(2, attemptNumber));
        }
    }
}
