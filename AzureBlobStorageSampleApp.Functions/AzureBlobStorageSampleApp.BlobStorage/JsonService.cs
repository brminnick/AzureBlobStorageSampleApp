using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace AzureBlobStorageSampleApp.Backend.Common
{
    public static class JsonService
    {
        #region Constant Fields
        static readonly Lazy<JsonSerializer> _serializerHolder = new Lazy<JsonSerializer>();
        #endregion

        #region Properties
        static JsonSerializer Serializer => _serializerHolder.Value;
        #endregion

        public static async Task<T> DeserializeHttpRequestMessage<T>(HttpRequestMessage requestMessage)
        {
            using (var stream = await requestMessage.Content.ReadAsStreamAsync().ConfigureAwait(false))
            using (var reader = new StreamReader(stream))
            using (var json = new JsonTextReader(reader))
            {
                if(json == null)
                        return default(T);

                return await Task.Run(() => Serializer.Deserialize<T>(json)).ConfigureAwait(false);
            }
        }
    }
}
