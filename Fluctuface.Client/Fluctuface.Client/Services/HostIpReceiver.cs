using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Haiku.Fluctuface.Client.Services
{
    public class HostIpReceiver
    {
        readonly UdpClient udp = new UdpClient(Constants.BroadcastPort);

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

            Debug.WriteLine("Received udp...");
            if (udpResult.Buffer.Length > 0)
            {
                string message = Encoding.UTF8.GetString(udpResult.Buffer);
                
                if (IPAddress.TryParse(message, out IPAddress _))
                {
                    Debug.WriteLine("Received host ip: " + message);
                    return message;
                }
            }

            return null;
        }
    }
}
