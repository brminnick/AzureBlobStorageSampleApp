using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace AzureBlobStorageSampleApp.Shared
{
    public static class JsonService
    {
        #region Constant Fields
        static readonly Lazy<JsonSerializer> _serializerHolder = new Lazy<JsonSerializer>();
        #endregion

        #region Properties
        static JsonSerializer Serializer => _serializerHolder.Value;
        #endregion

        public static async Task<T> DeserializeMessage<T>(HttpRequestMessage requestMessage)
        {
            using (var stream = await requestMessage.Content.ReadAsStreamAsync().ConfigureAwait(false))
                return await DeserializeContentStream<T>(stream).ConfigureAwait(false);
        }

        public static async Task<T> DeserializeMessage<T>(HttpResponseMessage responseMessage)
        {
            using (var stream = await responseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false))
                return await DeserializeContentStream<T>(stream).ConfigureAwait(false);
        }

        static async Task<T> DeserializeContentStream<T>(Stream contentStream)
        {
            using (var reader = new StreamReader(contentStream))
            using (var json = new JsonTextReader(reader))
            {
                if (json == null)
                    return default;

                return await Task.Run(() => Serializer.Deserialize<T>(json)).ConfigureAwait(false);
            }
        }
    }
}
