using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace Fluctuface.Server
{
    class HostIpSender
    {
        string ipAddress;

        internal HostIpSender()
        {
            ipAddress = GetLocalIpAddress();
        }

        internal void Send()
        {
            SendOnce();
        }

        void SendOnce()
        {
            Debug.WriteLine("Sending my ip: " + ipAddress);
            UdpClient client = new UdpClient();
            IPEndPoint ip = new IPEndPoint(IPAddress.Broadcast, Constants.BroadcastPort);

            byte[] bytes = Encoding.UTF8.GetBytes(ipAddress);
            client.Send(bytes, bytes.Length, ip);
            client.Close();
        }

        static string GetLocalIpAddress()
        {
            // https://stackoverflow.com/a/40528818
            UnicastIPAddressInformation mostSuitableIp = null;
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var network in networkInterfaces)
            {
                if (network.OperationalStatus != OperationalStatus.Up)
                    continue;

                var properties = network.GetIPProperties();
                if (properties.GatewayAddresses.Count == 0)
                    continue;

                foreach (var address in properties.UnicastAddresses)
                {
                    if (address.Address.AddressFamily != AddressFamily.InterNetwork)
                        continue;

                    if (IPAddress.IsLoopback(address.Address))
                        continue;

                    if (!address.IsDnsEligible)
                    {
                        if (mostSuitableIp == null)
                            mostSuitableIp = address;
                        continue;
                    }

                    // The best IP is the IP from the DHCP server
                    if (address.PrefixOrigin != PrefixOrigin.Dhcp)
                    {
                        if (mostSuitableIp == null || !mostSuitableIp.IsDnsEligible)
                            mostSuitableIp = address;
                        continue;
                    }
                    return address.Address.ToString();
                }
            }
            return mostSuitableIp != null
                ? mostSuitableIp.Address.ToString()
                : "0.0.0.0";
        }
    }
}
