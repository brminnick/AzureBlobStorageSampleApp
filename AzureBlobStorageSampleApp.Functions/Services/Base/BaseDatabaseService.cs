using System;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Threading.Tasks;

using NPoco;

namespace AzureBlobStorageSampleApp.Functions
{
    public abstract class BaseDatabaseService
    {
        #region Constant Fields
        readonly static string _connectionString = Environment.GetEnvironmentVariable("PhotoDatabaseConnectionString");
        #endregion

        protected static async Task<TResult> PerformDatabaseFunction<TResult>(Func<Database, Task<TResult>> databaseFunction) where TResult : class
        {
            using (var connection = new Database(_connectionString, DatabaseType.SqlServer2012, SqlClientFactory.Instance))
            {
                try
                {
                    return await databaseFunction?.Invoke(connection) ?? default;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("");
                    Debug.WriteLine(e.Message);
                    Debug.WriteLine(e.ToString());
                    Debug.WriteLine("");

                    throw;
                }
            }
        }
    }
}
