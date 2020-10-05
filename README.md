# Fluctuface
Live-edit variables in your desktop application with an iOS app.

Three components:
1. Fluctuface
Include this library in your application, mark assemblies with the `FluctuantAssembly` attribute and static float variables with the `Fluctuant` attribute. Call `FluctuantPatron.Instance.ExposeFluctuants()` when your application starts.
2. Fluctuface.Server

Expose static float variables to Fluctuface.Server. Live edit said variables with Fluctuface.Client iOS app on local network.

