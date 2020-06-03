# XFUserSecrets
Demo project that use UserSecrets in a Xamarin Forms App

## Description
This project demonstrate the use of UserSecrets to embed configuration data into a Xamarin Forms App without exposing them to the GitHub project.

This is obtained adding two MSBuild targets that copy the UserSecrets file to the project as an embeded resource prior to the build process, eventually deleting it at the end of build process.

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

## MSBuild "EmbedUserSecrets" target

To embed the `secret.json` file as an `EmbeddedResource` we need to do some steps before and after the build process.

Before the Build process:
1) Check that we are building a `debug` version;
2) Verify that the project is using UserSecrets;
3) Copy the `secret.json` file to the project folder;
4) Add the file to the `EmbeddedResource` file list.

After the Build process:
1) Delete the `secret.json` file.

Here is the `UserSecrets.targets` file that I've made to implement those steps:

```
<Project ToolsVersion="4.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <Target Name="EmbedUserSecrets" BeforeTargets="PrepareForBuild" Condition=" '$(Configuration)' == 'Debug' And '$(UserSecretsId)' != '' ">
    <PropertyGroup>
      <UserSecretsFilePath Condition=" '$(OS)' == 'Windows_NT' ">
        $([System.Environment]::GetFolderPath(SpecialFolder.UserProfile))\AppData\Roaming\Microsoft\UserSecrets\$(UserSecretsId)\secrets.json
      </UserSecretsFilePath>   
      <UserSecretsFilePath Condition=" '$(OS)' == 'Unix' ">
        $([System.Environment]::GetFolderPath(SpecialFolder.UserProfile))/.microsoft/usersecrets/$(UserSecretsId)/secrets.json
      </UserSecretsFilePath>
    </PropertyGroup>
    <Message Text="$(MSBuildThisFileDirectory)" />
    <Copy SourceFiles="$(UserSecretsFilePath)" DestinationFolder="$(MSBuildThisFileDirectory)"/>
    <ItemGroup>
      <EmbeddedResource Include="secrets.json" />
    </ItemGroup>
    <Message Text="UserSecretsFilePath: $(UserSecretsFilePath)" />
  </Target>
  <Target Name="DeleteUserSecrets" AfterTargets="Build" Condition=" '$(Configuration)' == 'Debug' And '$(UserSecretsId)' != '' ">
    <Message Text="TODO: Remove user secrets" />
    <Delete Files="secrets.json" />
  </Target>
</Project>
```

Then we need to import the `UserSecrets.targets` into the project `.csproj` file, just before the closing `</Project>` tag:

```
<?xml version="1.0" encoding="utf-8"?>
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>netstandard2.0</TargetFramework>
    <ProduceReferenceAssembly>true</ProduceReferenceAssembly>
    <UserSecretsId>18de31c9-1c19-4155-888e-ee0d3508551f</UserSecretsId>
  </PropertyGroup>
  <PropertyGroup Condition="'$(Configuration)|$(Platform)'=='Debug|AnyCPU'">
    <DebugType>portable</DebugType>
    <DebugSymbols>true</DebugSymbols>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Xamarin.Forms" Version="4.6.0.800" />
    <PackageReference Include="Xamarin.Essentials" Version="1.5.3.2" />
    <PackageReference Include="Newtonsoft.Json" Version="12.0.3" />
  </ItemGroup>
  <ItemGroup>
    <Folder Include="Utils\" />
  </ItemGroup>
    
  <Import Project="UserSecrets.targets" />
</Project>
```

That way, every time the Xamarin Forms common project is built and the conditions are meet, the `secrets.json` file will be embedded into the compiled project.

## Read the UserSecrets file from the App
Now that the `secrets.json` file has been embedded into the compiled Xamarin Forms common project, we can read his content from our app. As an example of reading from the embedded file, I've used the work done by Andrew Hoefling, well described on his post [Xamarin App Configuration: Control Your App Settings](https://www.andrewhoefling.com/Blog/Post/xamarin-app-configuration-control-your-app-settings) from where I've borrowed just a class that I've renamed `UserSecretsManager` that is called from the code behind of the `MainPage` to retrieve the value of `MySecret`.

Clearly this just a sample code, on a real app we'll have a mechanism where user secrets will be used to set/ovverride the configuration data on a debug configuration, and a CI/CD pipeline to inject the production configuration data on a release configuration.

## How to use UserSecrets in your Xamarin Forms app

To use User Secrets in your Xamarin Forms app you need to:

1) Init the UserSecrets with Visual Studio (only on your PC) or .NET Core CLI (on your PC or Mac);
2) Add the `UserSecrets.targets` file at the root of your Xamarin Forms common project;
3) Modify the `.csproj` file adding `<Import Project="UserSecrets.targets" />` just before the closing `</Project>` tag.

Then you can copy and use the `UserSecretsManager` class to read the secrets.json embedded file content ore create something more suited to your needs.
