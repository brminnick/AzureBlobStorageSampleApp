using SQLite;

namespace AzureBlobStorageSampleApp
{
	public interface ISQLite
	{
		SQLiteAsyncConnection GetConnection();
	}
}

