using System;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Plugin.Connectivity;
using Xamarin.Essentials;

namespace Fluctuface.Client
{
    public class HostFinder
    {
        const string getRestUrl = "http://{0}:52596/api/FluctuantVariables";
        const string putRestUrl = "http://{0}:52596/api/FluctuantVariables/{1}";
        readonly HttpClient client;
        string hostIp;
        CountdownEvent countdown;

        public HostFinder(HttpClient client)
        {
            this.client = client;
        }

        internal string GetRestUrl()
        {
            EnsureWeHaveHostIp();
            return string.Format(getRestUrl, hostIp);
        }

        internal string PutRestUrl(string itemId)
        {
            EnsureWeHaveHostIp();
            return string.Format(putRestUrl, hostIp, itemId);
        }

        void EnsureWeHaveHostIp()
        {
            //if (DeviceInfo.DeviceType == DeviceType.Physical)
            //{
            //    if (hostIp == null)
            //    {
            //        if (countdown == null)
            //        {
            //            FindHost();
            //        }
            //        countdown.Wait();
            //        countdown = null;
            //        EnsureWeHaveHostIp();
            //    }
            //}
            //else
            {
                hostIp = "192.168.0.3";
            }
        }

        void FindHost()
        {
            var connectivity = CrossConnectivity.Current;
            countdown = new CountdownEvent(1);
            var localIp = GetLocalIpAddress();
            var ipBase = localIp.Substring(0, localIp.LastIndexOf('.') + 1);
            var port = 52596;
            var batchSize = 6;

            for (int i = 1; i < 255; i++)
            {
                string ip = ipBase + i.ToString();

                if (ip != localIp)
                {
                    //Ping p = new Ping();
                    //p.PingCompleted += new PingCompletedEventHandler(p_PingCompleted);
                    countdown.AddCount();
                    try
                    {
                        //p.SendPingAsync(ip, 100).ContinueWith(pingTask =>
                        Console.WriteLine("Pinging " + ip);
                        connectivity.IsReachable(ip, 5000).ContinueWith(pingTask =>
                        {
                            if (pingTask.IsFaulted)
                            {
                                countdown.Signal();
                                Console.WriteLine("Faulted: " + ip);
                            }
                            else
                            {
                                var isReachable = pingTask.Result;
                                //string ip = (string)e.UserState;

                                if (isReachable)
                                {
                                    Console.WriteLine("{0} is up: ({1} ms)", ip);
                                    var uri = new Uri(string.Format(getRestUrl, ip));

                                    client.GetAsync(uri).ContinueWith(task =>
                                    {
                                        var response = task.Result;

                                        if (response.IsSuccessStatusCode)
                                        {
                                            Console.WriteLine("Found host ip: " + ip);
                                            hostIp = ip;
                                        }
                                        countdown.Signal();
                                    });
                                }
                                else// if (e.Reply == null)
                                {
                                    Console.WriteLine("Pinging {0} failed. (Null Reply object?)", ip);
                                    countdown.Signal();
                                }
                            }
                        });
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Exception " + ip);
                        countdown.Signal();
                    }

                    if ((i % batchSize) == 0)
                    {
                        while (countdown.CurrentCount > 1)
                        {
                            countdown.Wait(100 * batchSize);
                            if (hostIp != null)
                            {
                                break;
                            }
                        }
                    }
                    if (hostIp != null)
                    {
                        break;
                    }
                }
            }
            countdown.Signal();
        }

        void p_PingCompleted(object sender, PingCompletedEventArgs e)
        {
            string ip = (string)e.UserState;
            if (e.Reply != null && e.Reply.Status == IPStatus.Success)
            {
                Console.WriteLine("{0} is up: ({1} ms)", ip, e.Reply.RoundtripTime);
                var uri = new Uri(string.Format(getRestUrl, ip));

                client.GetAsync(uri).ContinueWith(task =>
                {
                    var response = task.Result;

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Found host ip: " + ip);
                        hostIp = ip;
                    }
                    countdown.Signal();
                });
            }
            else// if (e.Reply == null)
            {
                Console.WriteLine("Pinging {0} failed. (Null Reply object?)", ip);
                countdown.Signal();
            }
        }

        public static string GetLocalIpAddress()
        {
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

                    // The best IP is the IP got from DHCP server  
                    //if (address.PrefixOrigin != PrefixOrigin.Dhcp)
                    //{
                    //    if (mostSuitableIp == null || !mostSuitableIp.IsDnsEligible)
                    //        mostSuitableIp = address;
                    //    continue;
                    //}
                    //return address.Address.ToString();
                    mostSuitableIp = address;
                }
            }
            return mostSuitableIp != null
                ? mostSuitableIp.Address.ToString()
                : "0.0.0.0";
        }
    }
}
