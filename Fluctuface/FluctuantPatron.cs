using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Fluctuface
{
    public class FluctuantPatron
    {
        public List<FluctuantVariable> flucts;
        public Dictionary<string, FieldInfo> fluctuantFields = new Dictionary<string, FieldInfo>();

        public void ExposeFluctuants()
        {
            flucts = new List<FluctuantVariable>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                flucts.AddRange(GetFluctuants(assembly));
            }

            var pipe = new NamedPipeClientStream(".", "Fluctuface.Pipe", PipeDirection.InOut, PipeOptions.None);
            Console.WriteLine("Connecting");
            pipe.ConnectAsync().ContinueWith(task =>
            {
                Console.WriteLine($"Connected, sending variables {flucts.Count}");

                //var something = new byte[5] { 1, 2, 3, 4, 5 };

                //pipe.Write(something, 0, something.Length);

                //var tinyTot = new TinyTot() { Number = 1234 };

                //using (Utf8JsonWriter writer = new Utf8JsonWriter(pipe))
                //{
                //    JsonSerializer.Serialize(writer, tinyTot);
                //}

                //JsonSerializer.SerializeAsync(pipe, tinyTot).ContinueWith(serializeTask =>
                //                    {
                //                        Console.WriteLine("Sent serialized tinyTot");
                //                    });
                //foreach (var variable in flucts)
                //{
                //    JsonSerializer.SerializeAsync(pipe, variable).ContinueWith(serializeTask =>
                //    {
                //        Console.WriteLine("Sent serialized variables. Waiting for updates");
                //    });
                //}


                JsonSerializer.SerializeAsync(pipe, flucts).ContinueWith(serializeTask =>
                {
                    Console.WriteLine("Sent serialized variables. Waiting for updates");
                    //while (true)
                    //{
                    //    if (pipe.IsConnected)
                    //    {
                    //        JsonSerializer.DeserializeAsync<FluctuantVariable>(pipe).AsTask().ContinueWith(deserializeTask =>
                    //        {
                    //            Console.WriteLine("Deserialized");
                    //            if (!deserializeTask.IsFaulted)
                    //            {
                    //                var variable = deserializeTask.Result;
                    //                Console.WriteLine($"Got an update for {variable.Name}");

                    //                if (fluctuantFields.ContainsKey(variable.Id))
                    //                {
                    //                    Console.WriteLine($"Setting {variable.Name} to {variable.Value}");
                    //                    fluctuantFields[variable.Id].SetValue(null, variable.Value);
                    //                }
                    //            }
                    //        });
                    //    }
                    //}
                });
            });
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
