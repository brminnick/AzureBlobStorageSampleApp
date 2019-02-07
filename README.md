# AzureBlobStorageSampleApp

This Xamarin app uses a [SQLite Database](https://github.com/praeclarum/sqlite-net) to save the metadata of the Photos (e.g. Url, Title) locally. The local database syncs, via an [Azure Function](https://aka.ms/XamarinBlog/AzureFunctions), with an [Azure SQL Database](https://aka.ms/XamarinBlog/AzureSQL) that contains the metadata of the Photos stored in [Azure Blob Storage](https://aka.ms/xamarinblog/azureblobstorage).

The Xamarin app also allows the user to take photos and save them to [Azure Blob Storage](https://aka.ms/xamarinblog/azureblobstorage). To do this, the Xamarin app uploads the image to an [Azure Function](https://aka.ms/XamarinBlog/AzureFunctions), and the [Azure Function](https://aka.ms/XamarinBlog/AzureFunctions) saves the image in [Azure Blob Storage](https://aka.ms/xamarinblog/azureblobstorage), then adds the image metadata to the [Azure SQL Database](https://aka.ms/XamarinBlog/AzureSQL). 

## Learn More
- [The Xamarin Show: Azure Blob Storage for Mobile](https://channel9.msdn.com/Shows/XamarinShow/Azure-Blob-Storage-for-Mobile-with-Brandon-Minnick/?WT.mc_id=none-github-bramin)
- [Xamarin Blog: Add Cloud Storage to Xamarin Apps with Azure Blob Storage](https://blog.xamarin.com/xamarin-plus-azure-blob-cloud-storage/?WT.mc_id=none-ch9-bramin)
- [Azure Blob Storage](https://aka.ms/xamarinblog/azureblobstorage)
- [How to use Blob Storage from Xamarin](https://aka.ms/XamarinBlog/AzureBlobStorageWithXamarin)

![Azure Blob Storage Sample App Diagram](https://github.com/brminnick/Videos/blob/master/AzureBlobStorageSampleApp/AzureBlobStorageSampleAppDiagram.png)

## Getting Started

### 1. Publish The Function App to Azure

![](https://user-images.githubusercontent.com/3628580/52371778-68fb4e00-2a24-11e9-9617-bc9d6580bd8f.png)

1. In Visual Studio, right-click on AzureBlobStorageSampleApp.Functions and select Publish

![](https://user-images.githubusercontent.com/3628580/52371889-af50ad00-2a24-11e9-8c78-f3c8b5141566.png)

2. Choose Azure Function App -> Create New -> Publish

![](https://user-images.githubusercontent.com/3628580/52372329-eb384200-2a25-11e9-9833-d26397201523.png)

3. Fill out details: 
- App Name: Pick a name for your app within Azure
- Subscription: Select your Azure subscription 
- Resource Group: Pick a resource group or create a new one
- Hosting Plan: Pick a name, a region close to you, and for Size I chose Consumption 
- Storage Account: Create a new one. Pick a name, for Account Type I chose: Standard - Locally Redudant Storage


3. Click Create 

4. This will take a couple minutes to deploy; confirm its existance in the Azure portal.  We're done with the Function for now, but we'll be back to grab a couple values and add a couple values to the Application Settings.

### 2. Create Azure SQL Database

![](https://user-images.githubusercontent.com/13558917/29196780-9324ac1c-7deb-11e7-9d87-8a95ab62b0c5.png)

1. In the Azure portal, click on New -> Enter `SQL Database` into the Search Bar -> Select `SQL Database` from the search results -> Click Create

![](https://user-images.githubusercontent.com/13558917/29197883-2b850292-7df4-11e7-8bfd-8016d72f799a.png)

2. Name the SQL Database
- I named mine XamListDatabase
3. Select the Subscription
- I selected my Visual Studio Enterprise subscription
- If you do not have a VS Enterprise Subscription, you will need to select a different option
4. Select the Resource Group you published your Function in
5. Select Blank Database

![](https://user-images.githubusercontent.com/13558917/29198124-efa3b08c-7df5-11e7-87f4-42cf0dc95862.png)
6. Select Server

7. Select Create New Server

8. Enter the Server Name

9. Create a Server admin login
- Store this password somewhere safe, because we will need to use it for our database connection later!

10. Create a password

11. Select the closest location

12. Click "Select"

13. Select "Not Now" for the SQL Elastic Pool option

![](https://user-images.githubusercontent.com/13558917/29198240-f8b25cae-7df6-11e7-8f76-b8977645a712.png)
14. Select Pricing Tier
1. Select Basic
2. Move the slider to maximum, 2GB
- Don't worry, it's the same price for 2GB as it is for 100MB.
3. Click Apply
15. Click Create

### 3. Create the PhotoModel Table in the SQL Database

1. In the Azure Portal, navigate to the SQL Database we created in a previous step

![](https://user-images.githubusercontent.com/3628580/52374023-32283680-2a2a-11e9-9b66-ff0fe65227f7.png)

2. Select Query editor (preview).  Using Authorization type: SQL server Authentication, login with the server credentials we created in the previous step.

![image](https://user-images.githubusercontent.com/3628580/52374674-d9599d80-2a2b-11e9-88fb-4a97eba4e333.png)

3. In the query field, copy and paste the following.  And then Click Run.  

```
CREATE TABLE PhotoModels(
Id varchar(128) NOT NULL PRIMARY KEY,
CreatedAt datetimeoffset(7) NOT NULL,
UpdatedAt datetimeoffset(7) NOT NULL,
IsDeleted bit NOT NULL,
Url varchar(255) NOT NULL,
Title varchar(128) NOT NULL
);
```
![image](https://user-images.githubusercontent.com/3628580/52374508-749e4300-2a2b-11e9-92b4-1d0814087d0f.png)

4. If successful, when you click Tables, you should see the above Table. If you don't see anything immediately, try refreshing the web page.

### 4. Get SQL Database Connection String

![](https://user-images.githubusercontent.com/13558917/29198409-9d0dcab2-7df8-11e7-8c41-4797228ee4ab.png)

1. On the Azure Portal, navigate to the SQL Database we created, above
2. Click on "Connection Strings" -> "ADO.NET"
3. Copy the entire Connection String into a text editor

![](https://user-images.githubusercontent.com/13558917/29198528-b26f19f0-7df9-11e7-82c2-b4d46f60389a.png)

4. In the text editor, change "{your_username}" and "{your_password}" to match the SQL Database Username / Password created above
- Don't use my username / password because it won't work ;-)

### 5. Connect SQL Database to the Azure Function App

![](https://user-images.githubusercontent.com/13558917/29198794-f3673e5e-7dfb-11e7-89fc-ee042fe34704.png)

1. On the Azure Portal, navigate to the Functions App we published from Visual Studio
2. Select "Application Settings"
3. In the Application Settings, scroll down to the section "Application Settings"
4. Create a new string
- Set the name as `PhotoDatabaseConnectionString`
- Make sure to use this _exact_ name, otherwise the source code will not work
- Copy/paste the Azure SQL connection string from the text editor as the corresponding value
5. Scroll up and click Save (Note! If you don't click Save - the change will not be reflected.)

### 6. Create a (Blob) Storage Account

![](https://user-images.githubusercontent.com/3628580/52376722-d57c4a00-2a30-11e9-8c09-1374df8de3db.png)

1. In the Azure portal, click on New -> Enter `Storage account` into the Search Bar -> Select `Storage acount` from the search results -> Click Create 

![](https://user-images.githubusercontent.com/3628580/52378151-f050bd80-2a34-11e9-8914-d75f204da6e2.png)

2. In the next screen, you'll enter a few values
- Select the Subscription and the Resource Group you have been working in
- Create a Storage account name (note: letters in the name need to be lowercase) 
- Choose a location
- For Performance, I chose Standard
- For Account king, I chose StorageV2 (general purchase v2)
- For Access tier, chose Hot

3. Click Review and Create

### 7. Create a Blob container
1. Click into the Storage account you created.

![image](https://user-images.githubusercontent.com/3628580/52377203-2cceea00-2a32-11e9-840c-4cdf66c7d90f.png)

2. On the left menu, under Blob service, click Blobs

3. Click the "+ Container" button to create a new container

4. Use "photos" for the Name, and for the purposes of this exercise, chose Public access level: Container (anonymous read access for containers and blob)
- In future apps, you'll likely want to increase the privacy of your blob containers

### 8. Connect the (Blob) Storage Account to the Azure Function App

![](https://user-images.githubusercontent.com/3628580/52377592-5f2d1700-2a33-11e9-90e1-7703890ce69a.png)

1. In the Storage Account, click Access Keys which are under Settings

2. You'll see key 1 and key 2 along with a Key and Connection String for each of those.  Copy either of the Connection Strings.

![](https://user-images.githubusercontent.com/13558917/29198794-f3673e5e-7dfb-11e7-89fc-ee042fe34704.png)

1. On the Azure Portal, navigate to the Functions App we published from Visual Studio

2. Select "Application Settings"

3. In the Application Settings, scroll down to "Application Settings"

4. Create a new setting
- Set the name as `BlobStorageConnectionString`
- Make sure to use this _exact_ name, otherwise the source code will not work
- Copy/paste the Connection setting 

5. Create another setting
- Set the name as `PhotoContainerName`
- Make sure to use this _exact_ name, otherwise the source code will not work
- Type `photos` as the corresponding value (This is the name of the container you created earlier.) 

6. Scroll up and click Save (Note! If you don't click Save - the change will not be reflected.)

### 11. Configure Azure Function Url & Keys for Mobile App

![](https://user-images.githubusercontent.com/3628580/52378689-5c7ff100-2a36-11e9-81ff-caf04aa70767.png)

1. In [BackendConstants.cs](https://github.com/brminnick/AzureBlobStorageSampleApp/blob/master/AzureBlobStorageSampleApp.Mobile.Shared/Constants/BackendConstants.cs), you'll need to customize the value of `FunctionsAPIBaseUrl` to match yours
- Notice the URL in the upper right of the photo.  You'll only need to change the subdomain to match yours (ie. it is important that the URL in code retains the `/api`).

![](https://user-images.githubusercontent.com/3628580/52378976-2abb5a00-2a37-11e9-9fd0-5cbb68e01439.png)

2. In [BackendConstants.cs](https://github.com/brminnick/AzureBlobStorageSampleApp/blob/master/AzureBlobStorageSampleApp.Mobile.Shared/Constants/BackendConstants.cs), change the value of `PostPhotoBlobFunctionKey` to match your Azure Function Key for PostBlob
- As in the image above, click Manage under the PostBlob function.  Then under the section `Function Keys`, you'll see a function key with the name `default`.  You can `Click to show` to see the value or simply click the `Copy` action.  Careful that you specifically use the one under the section `Function Keys`.
