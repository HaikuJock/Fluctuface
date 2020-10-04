using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Fluctuface.Server
{
    public class HostIpSender
    {
        public void Send()
        {
            UdpClient client = new UdpClient();
            IPEndPoint ip = new IPEndPoint(IPAddress.Broadcast, 15000);

            byte[] bytes = Encoding.UTF8.GetBytes("Foo");
            client.Send(bytes, bytes.Length, ip);
            client.Close();
        }
    }
}
