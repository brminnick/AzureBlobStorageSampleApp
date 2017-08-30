using System;
using System.Configuration;

using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;

namespace AzureBlobStorageSampleApp.BlobStorage
{
    public static class BlobStorageService
    {
        #region Constant Fields
        readonly static Lazy<string> _connectionStringHolder = new Lazy<string>(() => ConfigurationManager.ConnectionStrings["BlobStorageConnectionString"].ConnectionString);
        readonly static Lazy<CloudStorageAccount> _storageAccountHolder = new Lazy<CloudStorageAccount>(() => CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting(ConnectionString)));
        #endregion

        #region Properties
        static string ConnectionString => _connectionStringHolder.Value;
        static CloudStorageAccount StorageAccount => _storageAccountHolder.Value;
        #endregion

        #region Methods
        public static void SavePhotoToBlobStorage()
        {
            
        }
#endregion
    }
}
