Example of waiting for assemblies to load before exposing fluctuants:
```
        static readonly HashSet<string> fluctuantAssemblies = new HashSet<string>
        {
            "My.Assembly.1",
            "My.Assembly.2",
        };
        static AssemblyLoadEventHandler assemblyLoadEventHandler;

        static void DebugInit()
        {
            var currentAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in currentAssemblies)
            {
                fluctuantAssemblies.Remove(assembly.FullName.Substring(0, assembly.FullName.IndexOf(',')));
            }
            assemblyLoadEventHandler = new AssemblyLoadEventHandler(OnAssemblyLoaded);
            AppDomain.CurrentDomain.AssemblyLoad += assemblyLoadEventHandler;
        }

        static void OnAssemblyLoaded(object sender, AssemblyLoadEventArgs args)
        {
            var assemblyShortName = args.LoadedAssembly.FullName.Substring(0, args.LoadedAssembly.FullName.IndexOf(','));
            fluctuantAssemblies.Remove(assemblyShortName);
            if (fluctuantAssemblies.Count == 0)
            {
                AppDomain.CurrentDomain.AssemblyLoad -= assemblyLoadEventHandler;
                assemblyLoadEventHandler = null;
                FluctuantPatron.Instance.ExposeFluctuants();
            }
        }
```
