# XFUserSecrets
Demo project that use UserSecrets in a Xamarin Forms App

## Description
This project demonstrate the use of UserSecrets to embed configuration data into a Xamarin Forms App without exposing them to the GitHub project.

Because the UserSecrets json data is embedded into the app, we can read it easily.
Moreover, because the UserSecrets file is outside the git root folder, it's never pushed to the GitHub repository.

## Create a "UserSecrets" file
Visual Studio has an easy way to create UserSecrets by right clicking on the Xamarin Forms common project and select `Manage User Secrets` :

![image](https://user-images.githubusercontent.com/139274/83561767-ae486680-a518-11ea-8026-ad88f2626287.png)

Elas, Visual studio for Mac don't have any command to manage user secrets, but we can use the .NET Core CLI (Command-Line Interface):
1) Open the Terminal;
2) Change directory to the Xamarin Forms common project folder
3) Execute `dotnet user-secrets init`

![image](https://user-images.githubusercontent.com/139274/83564726-7db6fb80-a51d-11ea-8c86-15da347bd0b3.png)

Using the .NET CLI, we can also add one or more secrets, like with `dotnet user-secrets set "MySecret" "HGttG:42"` :

![image](https://user-images.githubusercontent.com/139274/83570046-b0fd8880-a525-11ea-8c98-faae91840fdc.png)

More info on user secrets can be found in the [Microsoft documentation](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-3.1).

## The "secrets.json" file

The good thing of using user secrets is that a `secrets.json file` is created and stored outside of the solution folder in a place not managed by the git versioning system.
- On Windows platform the path is `%APPDATA%\Microsoft\UserSecrets\<user_secrets_id>\secrets.json`
- On Unix and Mac OSX platforms the path is `~/.microsoft/usersecrets/<user_secrets_id>/secrets.json`

The `UserSecretsId` is a Guid (Globally Unique Identifier) assigned during the user secrets initialization and stored on the .csproj file:

![image](https://user-images.githubusercontent.com/139274/83566339-236b6a00-a520-11ea-855a-d0648e953b80.png)

## MSBuild customization

To add the `secret.json` file as an `EmbeddedResource` to the Xamarin Forms common project we need to execute some steps before the build process:

1) Check that we are building a `debug` version;
2) Verify that the project is using UserSecrets;
3) Add the file to the `EmbeddedResource` file list.

In order to do that, we have multiple choices:

- Modify the `.csproj` file
- Create a `.targets` file and add an `import` command at the end of the `.csproj` file
- Create a `Directory.Build.targets` file on the same folder of the `.csproj` file 

The last one is preferable because we only needs to add a file that we can just copy and paste on every project where we want to use UserSecrets, without touching the `.csproj` file.

When MSBuild runs, *Microsoft.Common.targets* searches the directory structure for the `Directory.Build.targets` file. If it finds one, it imports the targets without the need to explicitly import them on the `.csproj` file. More info about the `Directory.Build.props`and `Directory.Build.targets` files can be found on [Microsoft Docs](https://docs.microsoft.com/en-us/visualstudio/msbuild/customize-your-build).

Here is the `Directory.Build.targets` file that I've made to implement those steps (thanks to [Jonathan Dick](https://twitter.com/redth) help):

```
<Project>
  <Target Name="AddUserSecrets"
          BeforeTargets="PrepareForBuild"
          Condition=" '$(Configuration)' == 'Debug' And '$(UserSecretsId)' != '' ">
    <PropertyGroup>
      <UserSecretsFilePath Condition=" '$(OS)' == 'Windows_NT' ">
        $([System.Environment]::GetFolderPath(SpecialFolder.UserProfile))\AppData\Roaming\Microsoft\UserSecrets\$(UserSecretsId)\secrets.json
      </UserSecretsFilePath>   
      <UserSecretsFilePath Condition=" '$(OS)' == 'Unix' ">
        $([System.Environment]::GetFolderPath(SpecialFolder.UserProfile))/.microsoft/usersecrets/$(UserSecretsId)/secrets.json
      </UserSecretsFilePath>
    </PropertyGroup>
    <ItemGroup>
      <EmbeddedResource Include="$(UserSecretsFilePath)" Condition="Exists($(UserSecretsFilePath))"/>
    </ItemGroup>
  </Target>
</Project>
```

It's worth noting that this works on both Windows and Unix/OSX platforms and that it refers the `UserSecretsId` property set on the .csproj file, so it can be copied and used in any solution as his. without any need to change it.

That way, every time the Xamarin Forms common project is built and the conditions are meet, the `secrets.json` file will be embedded into the compiled project.

## Read the "secrets.json" file from the App
Now that the `secrets.json` file has been embedded into the compiled Xamarin Forms common project, we can read his content from our app. As an example of reading from the embedded file, I've used the work done by Andrew Hoefling, well described on his post [Xamarin App Configuration: Control Your App Settings](https://www.andrewhoefling.com/Blog/Post/xamarin-app-configuration-control-your-app-settings) from where I've borrowed just a class that I've renamed `UserSecretsManager`, called from the code behind of the `MainPage` to retrieve the value of `MySecret`.

Clearly this just a sample code, on a real app we'll have a mechanism where user secrets will be used to set/ovverride the configuration data on a debug configuration, and a CI/CD pipeline to inject the production configuration data on a release configuration.

## How to use UserSecrets in your Xamarin Forms app

To use User Secrets in your Xamarin Forms app you need to:

1) Init the UserSecrets with Visual Studio (only on your PC) or .NET Core CLI (on your PC or Mac);
2) Add the `Directory.Build.targets` file at the root of your Xamarin Forms common project;

Then you can copy and use the `UserSecretsManager` class to read the secrets.json embedded file content or create something more suited to your needs.

In any case, using the UserSecrets you'll be sure that sensible data won't be inadvertently pushed to the GitHub repository anymore.
