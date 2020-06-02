# XFUserSecrets
Demo project that use UserSecrets in a Xamarin Forms App

## Description
This project demonstrate the use of UserSecrets to embed configuration data into a Xamarin Forms App without exposing them to the GitHub project.

This is obtained adding a MSBuild target that copy the UserSecrets file to the project as an embeded resource prior to the build process, eventually deleting it at the end of build process.

Because the UserSecrets json data is embedded into the app, we can read it easily.
Moreover, because the UserSecrets file is outside of the git root folder, is never committed nor pushed to the GitHub repository.

## MSBuild "EmbedUserSecrets" target

TODO

## Read the UserSecrets file from the App

TODO

## How to use UserSecrets in your Xamarin Forms app

TODO
