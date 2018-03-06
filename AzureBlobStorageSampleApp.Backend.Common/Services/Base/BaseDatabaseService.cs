using System;
using System.Data.Linq;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace AzureBlobStorageSampleApp.Backend.Common
{
    public abstract class BaseDatabaseService
    {
        protected static async Task<TResult> PerformDatabaseFunction<TResult>(Func<DataContext, TResult> databaseFunction, string connectionString) where TResult : class
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();

                var dbContext = new DataContext(sqlConnection);

                var signUpTransaction = sqlConnection.BeginTransaction();
                dbContext.Transaction = signUpTransaction;

                try
                {
                    return databaseFunction?.Invoke(dbContext) ?? default(TResult);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("");
                    Debug.WriteLine(e.Message);
                    Debug.WriteLine(e.ToString());
                    Debug.WriteLine("");

                    return default(TResult);
                }
                finally
                {
                    dbContext.SubmitChanges();
                    signUpTransaction.Commit();
                    sqlConnection.Close();
                }
            }
        }
    }
}
