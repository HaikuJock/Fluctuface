# Fluctuface
Live-edit variables in your desktop application with an iOS app.

This is a prototype; it does enough to live-edit floats but it's probably not very stable and there are no tests.

Three components:
1. **Fluctuface.Server** Build and publish to a folder and run locally. When your application starts it will send the exposed variables to the server via a named pipe.
2. **Fluctuface** Include this library in your application, mark assemblies with the `FluctuantAssembly` attribute and static float variables with the `Fluctuant` attribute. Call `FluctuantPatron.Instance.ExposeFluctuants()` when your application starts. See the SampleApp solution for an example.
3. **Fluctuface.Client** A Xamarin app that connects to the server on the local network and creates a slider for each exposed variable. Adjusting the slider will change the variable in your application.

