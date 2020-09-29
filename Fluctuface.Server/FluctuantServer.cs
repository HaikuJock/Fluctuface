using Fluctuface.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Fluctuface.Server
{

    public class FluctuantServer
    {
        public List<FluctuantVariable> flucts;
        public Dictionary<string, FieldInfo> fluctuantFields = new Dictionary<string, FieldInfo>();


        public FluctuantServer()
        {
        }

        public void Start()
        {
            flucts = new List<FluctuantVariable>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                flucts.AddRange(GetFluctuants(assembly));
            }

        }

        private List<FluctuantVariable> GetFluctuants(Assembly assembly)
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
                            var fluctuantVariable = new FluctuantVariable(fluct, (float)field.GetValue(null));

                            fluctuants.Add(fluctuantVariable);
                            fluctuantFields[fluctuantVariable.Id] = field;
                        }
                    }
                }
            }

            return fluctuants;
        }
    }
}
