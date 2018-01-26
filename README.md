# AzureBlobStorageSampleApp
This app shows how to leverage Azure Functions to upload an image to Azure Blob Storage. 

When a user takes a photo using the app, it is sent as a `byte[]` to an Azure Function which saves the image to Azure Blob Storage. Once the file has been save to Azure Blob Storage, it is automatically assigned a direct-access URL that is saved along with the Photo Title to an Azure SQL Database.

When the app launches, it hits a second Azure Function that returns all of the results from the Azure SQL Database that contains the photo titles and associated URLs.

To get started, visit the [Microsoft Docs](https://aka.ms/B5frc5).
