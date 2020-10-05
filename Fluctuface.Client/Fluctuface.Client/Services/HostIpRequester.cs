using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fluctuface.Client.Services
{
    class HostIpRequester
    {
        HostIpReceiver hostIpReceiver;
        CancellationToken token;

        internal async Task<string> RequestHost()
        {
            hostIpReceiver = new HostIpReceiver();
            var tokenSource = new CancellationTokenSource();
            
            token = tokenSource.Token;

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Task.Factory.StartNew(SendServiceRequestMessage, token);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            var result = await hostIpReceiver.GetReceivedHost();

            tokenSource.Cancel();

            return result;
        }

        void SendServiceRequestMessage()
        {
            do
            {
                SendOneServiceRequestMessage();
                Thread.Sleep(3000);
            } while (!token.IsCancellationRequested);
        }

        void SendOneServiceRequestMessage()
        {
            Console.WriteLine("Requesting service...");
            UdpClient client = new UdpClient();
            IPEndPoint ip = new IPEndPoint(IPAddress.Broadcast, Constants.BroadcastPort);

            byte[] bytes = Encoding.UTF8.GetBytes(Constants.ServiceRequestMessage);
            client.Send(bytes, bytes.Length, ip);
            client.Close();
        }
    }
}
