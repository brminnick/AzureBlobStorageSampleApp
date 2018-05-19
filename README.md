# AzureBlobStorageSampleApp

This Xamarin app uses a [SQLite Database](https://github.com/praeclarum/sqlite-net) to save the metadata of the Photos (e.g. Url, Title) locally. The local database syncs, via an [Azure Function](https://aka.ms/XamarinBlog/AzureFunctions), with an [Azure SQL Database](https://aka.ms/XamarinBlog/AzureSQL) that contains the metadata of the Photos stored in [Azure Blob Storage](https://aka.ms/xamarinblog/azureblobstorage).

The Xamarin app also allows the user to take photos and save them to [Azure Blob Storage](https://aka.ms/xamarinblog/azureblobstorage). To do this, the Xamarin app uploads the image to an [Azure Function](https://aka.ms/XamarinBlog/AzureFunctions), and the [Azure Function](https://aka.ms/XamarinBlog/AzureFunctions) saves the image in [Azure Blob Storage](https://aka.ms/xamarinblog/azureblobstorage), then adds the image metadata to the [Azure SQL Database](https://aka.ms/XamarinBlog/AzureSQL). 

## Learn More
- [The Xamarin Show: Azure Blob Storage for Mobile](https://channel9.msdn.com/Shows/XamarinShow/Azure-Blob-Storage-for-Mobile-with-Brandon-Minnick/?WT.mc_id=none-github-bramin)
- [Xamarin Blog: Add Cloud Storage to Xamarin Apps with Azure Blob Storage](https://blog.xamarin.com/xamarin-plus-azure-blob-cloud-storage/?WT.mc_id=none-ch9-bramin)
- [Azure Blob Storage](https://aka.ms/xamarinblog/azureblobstorage)
- [How to use Blob Storage from Xamarin](https://aka.ms/XamarinBlog/AzureBlobStorageWithXamarin)

![Azure Blob Storage Sample App Diagram](https://github.com/brminnick/Videos/blob/master/AzureBlobStorageSampleApp/AzureBlobStorageSampleAppDiagram.png)
