using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Text.Json;

namespace Fluctuface.Server
{

    public class FluctuantServer
    {
        public List<FluctuantVariable> flucts = new List<FluctuantVariable>();
        NamedPipeServerStream pipe;
        byte[] buffer = new byte[8192];

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
                //while (pipe.Read(buffer) > 0)
                //{
                //    Console.WriteLine("Read something");
                //}

                //var byteCount = pipe.Read(buffer);
                //var reader = new Utf8JsonReader(buffer);
                //var thing = JsonSerializer.Deserialize<TinyTot>(ref reader);

                //Console.WriteLine($"Read {thing.Number}");

                //var byteCount = pipe.Read(buffer);
                //var reader = new Utf8JsonReader(buffer);
                //var thing = JsonSerializer.Deserialize<FluctuantVariable>(ref reader);

                //Console.WriteLine($"Read {thing.Name} {thing.Value}");

                var byteCount = pipe.Read(buffer);
                var reader = new Utf8JsonReader(buffer);
                var thing = JsonSerializer.Deserialize<List<FluctuantVariable>>(ref reader);

                flucts = thing;
                Console.WriteLine($"Read {thing.Count}");
                Console.WriteLine($"First: {thing[0].Name} {thing[0].Value}");

                //JsonSerializer.DeserializeAsync<TinyTot>(pipe).AsTask().ContinueWith(listOfVariablesTask =>
                //{
                //    Console.WriteLine("DeserializedAsync");
                //    if (!listOfVariablesTask.IsFaulted)
                //    {
                //        Console.WriteLine($"Received tot {listOfVariablesTask.Result.Number}");
                //        //flucts.Add(listOfVariablesTask.Result);
                //    }
                //});
                //JsonSerializer.DeserializeAsync<FluctuantVariable>(pipe).AsTask().ContinueWith(listOfVariablesTask =>
                //{
                //    Console.WriteLine("DeserializedAsync");
                //    if (!listOfVariablesTask.IsFaulted)
                //    {
                //        Console.WriteLine($"Received fluct {listOfVariablesTask.Result.Name}");
                //        flucts.Add(listOfVariablesTask.Result);
                //    }
                //});
                //JsonSerializer.DeserializeAsync<List<FluctuantVariable>>(pipe).AsTask().ContinueWith(listOfVariablesTask =>
                //{
                //    Console.WriteLine("DeserializedAsync");
                //    if (!listOfVariablesTask.IsFaulted)
                //    {
                //        Console.WriteLine($"Received list of {listOfVariablesTask.Result.Count} flucts");
                //        flucts = listOfVariablesTask.Result;
                //    }
                //});
            });
        }

        internal void SendUpdateToPatron(FluctuantVariable fluctuantVariable)
        {
            JsonSerializer.SerializeAsync(pipe, fluctuantVariable);
        }
    }
}
