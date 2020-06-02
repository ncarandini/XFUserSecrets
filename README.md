# XFUserSecrets
Demo project that use UserSecrets in a Xamarin Forms App

## Description
This project demonstrate the use of UserSecrets to embed configuration data into a Xamarin Forms App without exposing them to the GitHub project.

This is obtained adding a MSBuild target that copy the UserSecrets file to the project as an embeded resource prior to the build process, eventually deleting it at the end of build process.

Because the UserSecrets json data is embedded into the app, we can read it easily.
Moreover, because the UserSecrets file is outside of the git root folder, is never committed nor pushed to the GitHub repository.

## Create a "UserSecrets" file
Visual Studio has an easy way to create UserSecrets by right clicking on the Xamarin Forms common project and select `Manage User Secrets` :

![image](https://user-images.githubusercontent.com/139274/83561767-ae486680-a518-11ea-8026-ad88f2626287.png)

Elas, Visual studio for Mac don't have any command to manage user secrets, but we can use the .NET Core CLI (Command-Line Interface):
1) Open the Terminal;
2) Change directory to the Xamarin Forms common project folder
3) Execute `dotnet user-secrets init`

![image](https://user-images.githubusercontent.com/139274/83564726-7db6fb80-a51d-11ea-8c86-15da347bd0b3.png)

More info on user secrets can be found in the [Microsoft documentation](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-3.1).

## The "secrets.json" file

The good thing of using user secrets is that a `secrets.json file` is created and stored outside of the solution folder in a place not managed by the git versioning system.
- On Windows platform the path is `%APPDATA%\Microsoft\UserSecrets\<user_secrets_id>\secrets.json`
- On Unix and Mac OSX platform the path is `~/.microsoft/usersecrets/<user_secrets_id>/secrets.json`

The `UserSecretsId` is a Guid (Globally Unique Identifier) assigned during initialization (by Visual Studio and .NET Core command), stored on the .csproj file:

![image](https://user-images.githubusercontent.com/139274/83566339-236b6a00-a520-11ea-855a-d0648e953b80.png)

## MSBuild "EmbedUserSecrets" target

TODO

## Read the UserSecrets file from the App

TODO

## How to use UserSecrets in your Xamarin Forms app

TODO
