using System.Threading.Tasks;

using SQLite;

using Xamarin.Forms;

using AzureBlobStorageSampleApp.Shared;

namespace AzureBlobStorageSampleApp
{
    public abstract class BaseDatabase
    {
        #region Constant Fields
        static readonly SQLiteAsyncConnection _databaseConnection = DependencyService.Get<ISQLite>().GetConnection();
        #endregion

        #region Fields
        static bool _isInitialized;
        #endregion

        #region Methods
        protected static async ValueTask<SQLiteAsyncConnection> GetDatabaseConnection()
        {
            if (!_isInitialized)
				await Initialize().ConfigureAwait(false);
                
            return _databaseConnection;
        }

        static async Task Initialize()
        {
            await _databaseConnection.CreateTableAsync<PhotoModel>().ConfigureAwait(false);
            _isInitialized = true;
        }
        #endregion

    }
}
