using System;
using System.Collections.Generic;
using System.IO;
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
        NamedPipeClientStream pipe;
        StreamWriter streamWriter;

        public void ExposeFluctuants()
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnExit);
            
            flucts = new List<FluctuantVariable>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                flucts.AddRange(GetFluctuants(assembly));
            }

            pipe = new NamedPipeClientStream(".", "Fluctuface.Pipe", PipeDirection.InOut, PipeOptions.None);
            Console.WriteLine("Connecting");
            pipe.ConnectAsync().ContinueWith(task =>
            {
                Console.WriteLine($"Connected, sending variables {flucts.Count}");
                streamWriter = new StreamWriter(pipe);
                var json = JsonSerializer.Serialize(flucts);
                streamWriter.WriteLine(json);
                streamWriter.Flush();
                Console.WriteLine("Finished send");
                //streamWriter.Close();
                //JsonSerializer.SerializeAsync(pipe, flucts).ContinueWith(serializeTask =>
                //{
                //    Console.WriteLine("Sent serialized variables. Waiting for updates");
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
                //});
            });
        }

        public void OnExit()
        {
            OnExit(null, null);
        }
        
        void OnExit(object sender, EventArgs e)
        {
            Console.WriteLine("exit");
            if (pipe != null)
            {
                pipe.Dispose();
                pipe = null;
                streamWriter = null;
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
