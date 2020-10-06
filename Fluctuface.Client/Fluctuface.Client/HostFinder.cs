using Haiku.Fluctuface.Client.Services;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace Haiku.Fluctuface.Client
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
            while (hostIp == null)
            {
                await FindHost();
            }
        }

        async Task FindHost()
        {
            var hostIpRequester = new HostIpRequester();
            var ip = await hostIpRequester.RequestHost();

            Debug.WriteLine("Received host: " + ip);
            var uri = new Uri(string.Format(getRestUrl, ip, Constants.ServicePort));

            try
            {
                var response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Verified host ip: " + ip);
                    hostIp = ip;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"ERROR {0}", ex.Message);
            }
        }
    }
}
