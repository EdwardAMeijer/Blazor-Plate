**What actions are required before running the project?**

*  Open the `BinaryPlate` solution in Visual Studio and navigate to the `BinaryPlate.WebAPI` project.
*  Locate the `appsettings.Development.json` file if you are in development environment, or `appsettings.json*` file if you are in production environment, and check the locate the `Initial Catalog` in the `HangfireConnection`. This connection string is used to connect to the `Hangfire` database.
*  From `SQL Server Management Studio`, Create an empty database and ensure that its name matches the name specified in the `Initial Catalog` parameter of the `Hangfire` connection string.
*  From Visual Studio, set the `BinaryPlate.BlazorPlate`, and `BinaryPlate.WebAPI` projects as startup projects by right-clicking on the solution in the Solution Explorer, and then, choose `Multiple startup projects` and set the Action for each project to `Start`.
*  Press F5 to run the projects.

**How do I perform a database migration in .NET Core when adding, modifying, or removing entities or fields?**

*  Go to `BinaryPlate.WebAPI/appsettings.Development.json` and locate the default connection string. Check the name of the database specified in the connection string.
*  If the database already exists, make sure to delete it in SQL Server Management Studio (SSMS). Locate the database by name and delete it.
*  In Visual Studio, right-click on the `BinaryPlate.WebAPI` project in the Solution Explorer and select `Set as Startup Project`.
*  Go to Visual Studio and open the Package Manager Console by going to `Tools` > `NuGet Package Manager` > `Package Manager Console`.
*  In the Package Manager Console, ensure that the `Default project` is set to your project that contains the DbContext. For example, if your DbContext is located in the `BinaryPlate.Infrastructure` project, then you should select `BinaryPlate.Infrastructure` as the default project.
*  Run the following command to create a new migration: `Add-Migration <MigrationName>` 
*  Replace `<MigrationName>` with a meaningful name for your migration. You can use any name you like, as long as it's unique and descriptive. For example:
    `Add-Migration AddProductsTable`  
*  After running the `Add-Migration` command, you can run the following command to apply the migration to the database:
    `Update-Database` 
*  If you encounter any errors during the migration process, you can use the following command to revert the migration:
    `Remove-Migration` 
*  You can also use the following command to view the history of your migrations:
`Get-Migrations`

**What type of architectural design is used in BinaryPlate?**

BinaryPlate uses Clean Architecture, which is an architecture that emphasizes the separation of concerns and the independence of the layers within an application. This type of architecture helps developers to maintain a clear and organized project structure that is scalable and easy to modify. In Clean Architecture, the application is divided into several layers, including the Presentation layer, Application layer, Domain layer, and Infrastructure layer.

BinaryPlate's use of Clean Architecture allows for a clean separation of concerns, making it easy to modify and test the application's functionality. Each layer in the architecture is independent and can be modified without affecting the other layers. This type of design pattern is especially useful for large projects that require frequent updates and modifications.

BinaryPlate also includes several architectural features, such as the use of interfaces and dependency injection, which further enhances the modularity and maintainability of the application.

**What are the types of application settings that should be considered before running BinaryPlate?**

To ensure the smooth operation of BinaryPlate, you should consider the following application settings located in the AppOptions section within the `appsettings.json` file:

*   Identity Options
*   Token Options
*   File Storage Options


**Where can modifications be made to the default Application Settings in BinaryPlate?**

You can modify application settings in two ways:

*   From the Settings Menu within the BlazorPlate GUI environment.
*   By changing the values in the AppOptions section in the `appsettings.json` file within the `BinaryPlate.WebAPI` project.

**What is the difference between Access Token and Refresh Token in BinaryPlate?**

Access tokens are used in secure applications to ensure a user has access to the appropriate resources. These tokens typically have a limited lifetime to limit the amount of time an attacker can use a stolen token and prevent stale information. When access tokens expire or become invalid, applications need a new access token without prompting the user. To solve this problem, OAuth 2.0 introduced a refresh token. A refresh token allows an application to obtain a new access token without prompting the user.

**What storage providers are supported by BinaryPlate?**

BinaryPlate supports On-Premise Storage and Azure Storage.

**What is the difference between On-Premise Storage and Cloud Storage?**

*   On-Premise storage and cloud storage reside in two different locations. On-premise storage utilizes in-house hardware and software. That is, the hardware is owned and managed by the enterprise versus a cloud service provider.
*   Cloud storage resides in remote servers, across town or across the country. It is typically provided by one of the large cloud computing companies such as AWS, Microsoft Azure, or Google Cloud.

**How can I run Azure BLOB Storage along with BinaryPlate on my localhost machine?**

To run Azure BLOB Storage along with BinaryPlate on your localhost machine, you can use the Azurite emulator, which provides a local environment for testing Azure blob, queue storage, and table storage applications. Follow these steps:

The Azurite open-source emulator provides a free local environment for testing your Azure blob, queue storage, and table storage applications. When you're satisfied with how your application is working locally, switch to using an Azure Storage account in the cloud. The emulator provides cross-platform support on Windows, Linux, and macOS.

Follow the following instructions in order to run Azurite emulator on your localhost machine.

*   Install `Node.js version 8.0 or later.` (https://nodejs.org)
*   Use the npm command `npm install -g azurite` to install Azurite.
*   Start Azurite from the command line using the command `azurite`.
*   Run BlazorPlate and upload a new avatar picture to update your profile.
*   Refresh the page and inspect the image path by right-clicking on the avatar picture.
*   You should see that the image path points to the Azure BLOB server address, which is `http://127.0.0.1:10000/devstoreaccount1`.

**How can I run BinaryPlate with a real Azure Storage account in the cloud?**
*   To run BinaryPlate with a real Azure Storage account in the cloud, you need to follow these steps:
*   Login to your Azure account and obtain a new connection string for Azure BLOB storage.
*   Open the `appsettings.json` file located within the `BinaryPlate.WebAPI` project.
*   Find the `ConnectionStrings:AzureStorageConnection` section in the `appsettings.json` file.
*   Replace the ``UseDevelopmentStorage=true`` value with the real connection string that you've obtained from your Azure account.

By following these steps, you'll be able to run BinaryPlate with your Azure Storage account in the cloud.

**Can I add a new storage provider in BinaryPlate?**

Yes! BinaryPlate supports adding multiple storage providers. Simply create a new service class, for example, `AwsStorageService` that implements the `IFileStorageService` interface.

**How can I adjust the maximum file upload size limit in BinaryPlate?**

To change the maximum file upload size limit in BinaryPlate, you should update three places:

*   Update the `maxAllowedContentLength` value in web.config file located within the `BinaryPlate.WebAPI` project as shown in the following code snippet.

![](https://blazorplate.net/assets/img/code-screenshots/file-size-webconfig.png)

*   Update the `ValueLengthLimit` and `MultipartBodyLengthLimit` values in the `Startup.cs` file located within the `BinaryPlate.WebAPI` project as shown in the following code snippet.

![](https://blazorplate.net/assets/img/code-screenshots/file-size-startup.png)

*   Update the MaxFileSize value in the Shared/BpUploadFile.razor.cs file located within the `BinaryPlate.BlazorPlate` project as shown in the following code snippet.

![](https://blazorplate.net/assets/img/code-screenshots/file-size-bpuploadfile.png)
