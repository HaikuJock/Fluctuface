using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Fluctuface.Server
{
    class ClientListener
    {
        readonly UdpClient udp = new UdpClient(Constants.BroadcastPort);

        internal async Task Listen()
        {
            Console.WriteLine("Listening for clients on the network");
            do
            {
                var message = await ReceiveNextUdpMessage();
                if (message == Constants.ServiceRequestMessage)
                {
                    var sender = new HostIpSender();

                    sender.Send();
                }
            } while (true);
        }

        internal async Task<string> ReceiveNextUdpMessage()
        {
            var udpResult = await udp.ReceiveAsync();

            Console.WriteLine("Received udp...");
            if (udpResult.Buffer.Length > 0)
            {
                string message = Encoding.UTF8.GetString(udpResult.Buffer);

                Console.WriteLine("message : " + message);
                return message;
            }

            return null;
        }
    }
}
