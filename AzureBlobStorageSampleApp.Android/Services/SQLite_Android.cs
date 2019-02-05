using System.IO;

using SQLite;

using Xamarin.Forms;

using AzureBlobStorageSampleApp.Android;

[assembly: Dependency(typeof(SQLite_Android))]
namespace AzureBlobStorageSampleApp.Android
{
	public class SQLite_Android : ISQLite
    {
		#region ISQLite implementation
		public SQLiteAsyncConnection GetConnection()
		{
			var sqliteFilename = "AzureBlobStorageSampleApp.db3";
			string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			
            var path = Path.Combine(documentsPath, sqliteFilename);
            var connection = new SQLiteAsyncConnection(path, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);
			
            return connection;
		}
		#endregion
	}
}

