using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Reflection;
using System.Text.Json;

namespace Fluctuface
{
    public class FluctuantPatron
    {
        readonly List<FluctuantVariable> flucts = new List<FluctuantVariable>();
        readonly Dictionary<string, FieldInfo> fluctuantFields = new Dictionary<string, FieldInfo>();
        NamedPipeClientStream pipe;
        StreamWriter streamWriter;

        public void ExposeFluctuants()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                flucts.AddRange(GetFluctuants(assembly));
            }

            pipe = new NamedPipeClientStream(".", "Fluctuface.Pipe", PipeDirection.InOut, PipeOptions.None);
            Debug.WriteLine("Connecting");
            pipe.ConnectAsync().ContinueWith(task =>
            {
                Debug.WriteLine($"Connected, sending variables {flucts.Count}");
                streamWriter = new StreamWriter(pipe);
                var json = JsonSerializer.Serialize(flucts);
                streamWriter.WriteLine(json);
                streamWriter.Flush();
                Debug.WriteLine("Finished send");

                using (var streamReader = new StreamReader(pipe))
                {
                    while (true)
                    {
                        // TODO: Use a blocking call to the stream rather than a tight loop
                        if (!streamReader.EndOfStream)
                        {
                            string str = streamReader.ReadLine();

                            if (!string.IsNullOrEmpty(str))
                            {
                                var variable = JsonSerializer.Deserialize<FluctuantVariable>(str);

                                if (fluctuantFields.TryGetValue(variable.Id, out FieldInfo fieldInfo))
                                {
                                    fieldInfo.SetValue(null, variable.Value);
                                }
                            }
                            else
                            {
                                Debug.WriteLine("Empty pipe read!?");
                            }
                        }
                    }
                }
            });
        }

        List<FluctuantVariable> GetFluctuants(Assembly assembly)
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
                            Debug.WriteLine("Found a fluct!");
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
