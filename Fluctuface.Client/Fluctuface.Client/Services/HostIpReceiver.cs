//using System;
//using System.Net;
//using System.Net.Sockets;
//using System.Text;

//namespace Fluctuface.Client.Services
//{
//    public class HostIpReceiver
//    {
//        readonly UdpClient udp = new UdpClient(15000);

//        public HostIpReceiver()
//        {
//        }

//        void StartListening()
//        {
//            this.udp.BeginReceive(Receive, new object());
//        }

//        void Receive(IAsyncResult ar)
//        {
//            IPEndPoint ip = new IPEndPoint(IPAddress.Any, 15000);
//            byte[] bytes = udp.EndReceive(ar, ref ip);
//            string message = Encoding.UTF8.GetString(bytes);
//            StartListening();
//        }
//    }
//}
