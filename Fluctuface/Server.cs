using Fluctuface.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Fluctuface
{

    public class Server
    {
        public Server()
        {

        }

        public void Start()
        {
            var fluctuants = new List<FluctuantVariable>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                fluctuants.AddRange(GetFluctuants(assembly));
            }
        }

        private static List<FluctuantVariable> GetFluctuants(Assembly assembly)
        {
            var fluctuants = new List<FluctuantVariable>();

            if (assembly.GetCustomAttribute<FluctuantAssembly>() != null)
            {
                var types = assembly.GetTypes();

                foreach (var type in types)
                {
                    foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Static))
                    {
                        var fluct = field.GetCustomAttribute<Fluctuant>();

                        if (fluct != null)
                        {
                            Console.WriteLine("Found a fluct!");
                            var fluctuantVariable = new FluctuantVariable(fluct, field);

                            fluctuants.Add(fluctuantVariable);
                        }
                    }
                }

            }

            return fluctuants;
        }
    }
}
