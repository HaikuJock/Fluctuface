using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Fluctuface.Client.Services
{
    public class HostIpReceiver
    {
        readonly UdpClient udp = new UdpClient(Constants.BroadcastPort);

        public HostIpReceiver()
        {
        }

        //void StartListening()
        //{
        //    udp.BeginReceive(Receive, new object());
        //}

        //void Receive(IAsyncResult ar)
        //{
        //    IPEndPoint ip = new IPEndPoint(IPAddress.Any, Constants.BroadcastPort);
        //    byte[] bytes = udp.EndReceive(ar, ref ip);
        //    string message = Encoding.UTF8.GetString(bytes);

        //    StartListening();
        //}

        internal async Task<string> GetReceivedHost()
        {
            string hostIp;

            do
            {
                hostIp = await ReceiveNextUdpMessage();
            } while (hostIp == null);

            return hostIp;
        }

        internal async Task<string> ReceiveNextUdpMessage()
        {
            var udpResult = await udp.ReceiveAsync();

            Console.WriteLine("Received udp...");
            if (udpResult.Buffer.Length > 0)
            {
                string message = Encoding.UTF8.GetString(udpResult.Buffer);
                
                if (IPAddress.TryParse(message, out IPAddress _))
                {
                    Console.WriteLine("Received host ip: " + message);
                    return message;
                }
            }

            return null;
        }
    }
}
