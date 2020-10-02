using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Fluctuface.Server
{

    public class FluctuantServer
    {
        public List<FluctuantVariable> flucts = new List<FluctuantVariable>();
        NamedPipeServerStream pipe;

        public FluctuantServer()
        {
        }

        public void Start()
        {
            pipe = new NamedPipeServerStream("Fluctuface.Pipe", PipeDirection.InOut);
            Console.WriteLine("Waiting for connection....");
            pipe.WaitForConnectionAsync().ContinueWith(task =>
            {
                Console.WriteLine("Connected");
                JsonSerializer.DeserializeAsync<List<FluctuantVariable>>(pipe).AsTask().ContinueWith(listOfVariablesTask =>
                {
                    Console.WriteLine("DeserializedAsync");
                    if (!listOfVariablesTask.IsFaulted)
                    {
                        Console.WriteLine($"Received list of {listOfVariablesTask.Result.Count} flucts");
                        flucts = listOfVariablesTask.Result;
                    }
                });
            });
        }

        internal void SendUpdateToPatron(FluctuantVariable fluctuantVariable)
        {
            JsonSerializer.SerializeAsync(pipe, fluctuantVariable);
        }
    }
}
