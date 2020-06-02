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

Elas, Visual studio for Mac don't have any command to manage user secrets, but you can use the CLI:
1) Open the Terminal;
2) Change directory to the Xamarin Forms common project folder
3) Execute `dotnet user-secrets init`



## MSBuild "EmbedUserSecrets" target

TODO

## Read the UserSecrets file from the App

TODO

## How to use UserSecrets in your Xamarin Forms app

TODO
