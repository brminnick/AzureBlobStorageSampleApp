using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Polly;
using SQLite;
using Xamarin.Essentials;

namespace AzureBlobStorageSampleApp
{
	public abstract class BaseDatabase
	{
		static readonly string _databasePath = Path.Combine(FileSystem.AppDataDirectory, $"{nameof(AzureBlobStorageSampleApp)}.db3");

		static readonly Lazy<SQLiteAsyncConnection> _databaseConnectionHolder =
			new Lazy<SQLiteAsyncConnection>(() => new SQLiteAsyncConnection(_databasePath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache));

		static SQLiteAsyncConnection DatabaseConnection => _databaseConnectionHolder.Value;

		protected static async Task<TReturn> ExecuteDatabaseFunction<TDataType, TReturn>(Func<SQLiteAsyncConnection, Task<TReturn>> action, int numRetries = 12)
		{
			if (!DatabaseConnection.TableMappings.Any(x => x.MappedType == typeof(TDataType)))
            {
				await DatabaseConnection.EnableWriteAheadLoggingAsync().ConfigureAwait(false);
				await DatabaseConnection.CreateTablesAsync(CreateFlags.None, typeof(TDataType)).ConfigureAwait(false);
			}				

			return await Policy.Handle<Exception>().WaitAndRetryAsync(numRetries, pollyRetryAttempt).ExecuteAsync(() => action(DatabaseConnection)).ConfigureAwait(false);

			static TimeSpan pollyRetryAttempt(int attemptNumber) => TimeSpan.FromMilliseconds(Math.Pow(2, attemptNumber));
		}
	}
}
