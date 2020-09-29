using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Fluctuface
{
    class FluctuantVariable
    {
        public Fluctuant Fluctuant;
        public FieldInfo FieldInfo;
    }

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
                            var fluctuantVariable = new FluctuantVariable
                            {
                                FieldInfo = field,
                                Fluctuant = fluct,
                            };
                            fluctuants.Add(fluctuantVariable);
                            field.SetValue(null, 0.999f);
                        }
                    }
                }

            }

            return fluctuants;
        }
    }
}
