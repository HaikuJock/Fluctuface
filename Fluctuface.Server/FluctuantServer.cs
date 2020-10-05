using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Text.Json;
using System.Threading.Tasks;

namespace Fluctuface.Server
{
    class FluctuantServer
    {
        internal List<FluctuantVariable> flucts = new List<FluctuantVariable>();
        Dictionary<string, FluctuantVariable> latestValues = new Dictionary<string, FluctuantVariable>();
        NamedPipeServerStream connectedPipe;
        StreamWriter streamWriter;
        StreamReader streamReader;

        internal void Start()
        {
            CreatePatronThread();
            CreateServerDiscoveryThread();
        }

        internal void SendUpdateToPatron(FluctuantVariable fluctuantVariable)
        {
            if (connectedPipe != null)
            {
                lock (connectedPipe)    // added because patron received two pieces of json on the same line when client quickly changed slider
                {
                    Debug.WriteLine($"Sending {fluctuantVariable.Id} value: {fluctuantVariable.Value}");
                    latestValues[fluctuantVariable.Id] = fluctuantVariable;
                    if (streamWriter == null)
                    {
                        streamWriter = new StreamWriter(connectedPipe);
                    }

                    var json = JsonSerializer.Serialize(fluctuantVariable);

                    try
                    {
                        streamWriter.WriteLine(json);
                        streamWriter.Flush();
                        Debug.WriteLine("Sent");
                    }
                    catch (Exception)
                    {
                        Debug.WriteLine("Failed to send to client");
                        streamWriter = null;
                        streamReader = null;
                        connectedPipe = null;
                    }
                }
            }
        }

        void CreatePatronThread()
        {
            Task.Factory.StartNew(PatronThread);
        }

        void CreateServerDiscoveryThread()
        {
            Task.Factory.StartNew(ServerDiscoveryThread);
        }

        void PatronThread()
        {
            var pipe = new NamedPipeServerStream("Fluctuface.Pipe", PipeDirection.InOut, 2);
            Debug.WriteLine("Waiting for connection....");
            pipe.WaitForConnection();
            if (pipe.IsConnected)
            {
                if (connectedPipe?.IsConnected == true)
                {
                    connectedPipe.Disconnect();
                    try
                    {
                        connectedPipe.Close();
                        streamWriter?.Close();
                        streamReader?.Close();
                    }
                    catch (Exception)
                    {
                    }
                    connectedPipe = null;
                    streamWriter = null;
                    streamReader = null;
                }
                connectedPipe = pipe;
                Debug.WriteLine("Connected");
                CreatePatronThread();
                streamReader = new StreamReader(pipe);
                string str = streamReader.ReadLine();

                if (!string.IsNullOrEmpty(str))
                {
                    Debug.WriteLine("{0}", str);
                    var patronFlucts = JsonSerializer.Deserialize<List<FluctuantVariable>>(str);
                    var newFlucts = new List<FluctuantVariable>();

                    foreach (var fluct in patronFlucts)
                    {
                        if (latestValues.TryGetValue(fluct.Id, out FluctuantVariable latestFluct)
                            && latestFluct.Value >= fluct.Min
                            && latestFluct.Value <= fluct.Max)
                        {
                            fluct.Value = latestFluct.Value;
                            SendUpdateToPatron(fluct);
                        }
                        newFlucts.Add(fluct);
                    }
                    flucts = newFlucts;
                }
                else
                {
                    Debug.WriteLine("Nothing to read");
                }
            }
        }

        async Task ServerDiscoveryThread()
        {
            var listener = new ClientListener();

            await listener.Listen();
        }
    }
}
