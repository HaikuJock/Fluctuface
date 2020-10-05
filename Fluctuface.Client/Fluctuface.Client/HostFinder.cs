using System;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Fluctuface.Client.Services;
using Plugin.Connectivity;
using Xamarin.Essentials;

namespace Fluctuface.Client
{
    public class HostFinder
    {
        const string getRestUrl = "http://{0}:{1}/api/FluctuantVariables";
        const string putRestUrl = "http://{0}:{1}/api/FluctuantVariables/{2}";
        readonly HttpClient client;
        string hostIp;

        public HostFinder(HttpClient client)
        {
            this.client = client;
        }

        internal async Task<string> GetRestUrl()
        {
            await EnsureWeHaveHostIp();
            return string.Format(getRestUrl, hostIp, Constants.ServicePort);
        }

        internal async Task<string> PutRestUrl(string itemId)
        {
            await EnsureWeHaveHostIp();
            return string.Format(putRestUrl, hostIp, Constants.ServicePort, itemId);
        }

        async Task EnsureWeHaveHostIp()
        {
            //if (DeviceInfo.DeviceType == DeviceType.Physical)
            {
                while (hostIp == null)
                {
                    await FindHost();
                }
            }
            //else
            //{
            //    hostIp = "192.168.0.3";
            //}
        }

        async Task FindHost()
        {
            var hostIpRequester = new HostIpRequester();
            var ip = await hostIpRequester.RequestHost();

            Console.WriteLine("Received host: " + ip);
            var uri = new Uri(string.Format(getRestUrl, ip, Constants.ServicePort));

            try
            {
                var response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Verified host ip: " + ip);
                    hostIp = ip;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"ERROR {0}", ex.Message);
            }
        }
    }
}
