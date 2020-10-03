using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Text.Json;

namespace Fluctuface.Server
{

    public class FluctuantServer
    {
        public List<FluctuantVariable> flucts = new List<FluctuantVariable>();
        NamedPipeServerStream pipe;
        StreamReader streamReader;
        //byte[] buffer = new byte[8192];

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
                streamReader = new StreamReader(pipe);
                string str = streamReader.ReadLine();

                if (!string.IsNullOrEmpty(str))
                {
                    Console.Write("{0}", str);
                    flucts = JsonSerializer.Deserialize<List<FluctuantVariable>>(str);
                }
                else
                {
                    Console.WriteLine("Nothing to read");
                }

                //var byteCount = pipe.Read(buffer);

                //if (byteCount > 0)
                //{
                //    var reader = new Utf8JsonReader(buffer);

                //    flucts = JsonSerializer.Deserialize<List<FluctuantVariable>>(ref reader);
                //    Console.WriteLine($"Received {flucts.Count} fluctuant variable(s)");
                //}
                //else
                //{
                //    Console.WriteLine("Nothing to read");
                //}
            });
        }

        internal void SendUpdateToPatron(FluctuantVariable fluctuantVariable)
        {
            JsonSerializer.SerializeAsync(pipe, fluctuantVariable);
        }
    }
}
