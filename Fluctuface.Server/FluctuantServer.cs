using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Text.Json;
using System.Threading.Tasks;

namespace Fluctuface.Server
{

    public class FluctuantServer
    {
        public List<FluctuantVariable> flucts = new List<FluctuantVariable>();
        NamedPipeServerStream connectedPipe;
        StreamWriter streamWriter;
        StreamReader streamReader;

        public FluctuantServer()
        {
        }

        public void Start()
        {
            CreateListenerThread();
        }

        void CreateListenerThread()
        {
            Task.Factory.StartNew(ListenerThread);
        }

        void ListenerThread()
        {
            var pipe = new NamedPipeServerStream("Fluctuface.Pipe", PipeDirection.InOut, 2);
            Console.WriteLine("Waiting for connection....");
            pipe.WaitForConnection();
            if (pipe.IsConnected)
            {
                if (connectedPipe?.IsConnected == true)
                {
                    connectedPipe.Disconnect();
                    try
                    {
                        streamWriter.Close();
                        streamReader.Close();
                    }
                    catch (Exception)
                    {
                    }
                    streamWriter = null;
                    streamReader = null;
                }
                connectedPipe = pipe;
                Console.WriteLine("Connected");
                CreateListenerThread();
                streamReader = new StreamReader(pipe);
                string str = streamReader.ReadLine();

                if (!string.IsNullOrEmpty(str))
                {
                    Console.WriteLine("{0}", str);
                    flucts = JsonSerializer.Deserialize<List<FluctuantVariable>>(str);
                }
                else
                {
                    Console.WriteLine("Nothing to read");
                }
            }
        }

        internal void SendUpdateToPatron(FluctuantVariable fluctuantVariable)
        {
            if (connectedPipe != null)
            {
                Console.WriteLine($"Sending {fluctuantVariable.Id} value: {fluctuantVariable.Value}");
                if (streamWriter == null)
                {
                    streamWriter = new StreamWriter(connectedPipe);
                }

                var json = JsonSerializer.Serialize(fluctuantVariable);

                try
                {
                    streamWriter.WriteLine(json);
                    streamWriter.Flush();
                    Console.WriteLine("Sent");
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to send to client");
                    streamWriter = null;
                    streamReader = null;
                    connectedPipe = null;
                }
            }
        }
    }
}
