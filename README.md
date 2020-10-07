# Fluctu(ant Inter)face
Live-edit variables in your desktop application from an iOS device.

This is a prototype; it does enough to live-edit `static float`s but it's probably not very stable and there are no tests.

#### Motivation
The values of some constants are best determined aesthetically or by playing with them. In game development this is commonly achieved with an editor or debug menu but these get in the way of the experience, either visually or by a break in concentration. Fluctuface enables playing with constants from a second screen. Once set up, new constants can be exposed with a single line of code.

This was inspired by [LazerWalker's](https://twitter.com/lazerwalker) [GroundKontrol](https://github.com/lazerwalker/GroundKontrol). Watch her [excellent](https://www.youtube.com/watch?v=-aXrLvdrnao&t=23m51s) [talks](https://youtu.be/stM33UcLPJ0) for more on motivations.

#### Dependencies
* Visual Studio 2019 to build the server
* Visual Studio for Mac to build the iOS app
* .NET Core 3.1 to run the sample app
* .NET Standard 2.0 for the client library

#### Structure
Three components:
1. **Fluctuface** Client library containing the attributes to expose fluctuants. Sends and receives fluctuants to and from a `Fluctuface.Server` running on the local machine via a named pipe.
2. **Fluctuface.Server** Provides an http REST API to receive and update fluctuants.
3. **Fluctuface.Client** A Xamarin app that connects to the server on the local network and creates a slider for each exposed variable. Adjusting the slider changes the variable in the `Fluctuface` application. Host discovery is done with UDP broadcast.

#### Installation
1. Add the [Haiku.Fluctuate](https://www.nuget.org/packages/Haiku.Fluctuface) NuGet package to your application. Call `FluctuantPatron.Instance.ExposeFluctuants()` [when your assemblies are loaded](WaitForAssembliesSample.md). Add a `Fluctuant` attribute to a `static float` variable. For example:
```
        [Fluctuant("BlueTint", 0f, 1f)]
        public static float BlueTintMax = 0.16666f;
```
2. Clone this repository and build, publish and run the `Fluctuface.Server` application (or download and run this [Windows build](https://fluctuface.s3-eu-west-1.amazonaws.com/Fluctuface.Server-Win.zip))
3. Build the `Fluctuface.Client.iOS` app on a Mac and deploy to your iOS device. You will need to use your own development certificate and provisioning profile.

#### Usage
1. Launch the `Fluctuface.Server` (in your browser verify http://localhost:52596/api/FluctuantVariables is returning an empy array)
2. Launch your application
3. Launch the `Fluctuface` iOS app
4. For each `Fluctuant` in your application you should see a slider, changing the slider will update your application's variable.

You should be able to leave the server and iOS app running if you need to restart your application. Pull down on the iOS app to refresh.

#### Limitations
* The client library and server use named pipes. These don't work on Xamarin.Mac.
* There is almost no error-checking.
* The values are not saved anywhere, you'll need to manually update your code when you find the values you like.
* Changing the min or max of a variable requires a server restart

#### Contact
* https://github.com/HaikuJock
* https://twitter.com/haikuinteractiv
