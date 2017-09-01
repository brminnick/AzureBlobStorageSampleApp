using System;
using System.IO;

using SQLite;

using Xamarin.Forms;

using AzureBlobStorageSampleApp.iOS;

[assembly: Dependency(typeof(SQLite_iOS))]
namespace AzureBlobStorageSampleApp.iOS
{
	public class SQLite_iOS : ISQLite
	{
		#region ISQLite implementation
		public SQLiteAsyncConnection GetConnection()
		{
			var sqliteFilename = "AzureBlobStorageSampleApp.db3";
			var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var libraryPath = Path.Combine(documentsPath, "..", "Library");

            var path = Path.Combine(libraryPath, sqliteFilename);
            var conn = new SQLiteAsyncConnection(path, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);

			return conn;
		}
		#endregion
	}
}

