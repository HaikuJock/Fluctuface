using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Fluctuface.Client.Services
{
    public class RestService : IRestService
    {
        HttpClient client;
        HostFinder hostFinder;

        public List<FluctuantVariable> Items { get; private set; }

        public RestService()
        {
#if DEBUG
            client = new HttpClient(DependencyService.Get<IHttpClientHandlerService>().GetInsecureHandler());
#else
            client = new HttpClient();
#endif
            hostFinder = new HostFinder(client);
        }

        public async Task<IEnumerable<FluctuantVariable>> RefreshDataAsync()
        {
            var getRequest = await hostFinder.GetRestUrl();
            Uri uri = new Uri(string.Format(getRequest, string.Empty));

            Items = new List<FluctuantVariable>();
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Items = JsonConvert.DeserializeObject<List<FluctuantVariable>>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return Items;
        }

        public async Task<bool> SaveAsync(FluctuantVariable item)
        {
            var putRequest = await hostFinder.PutRestUrl(item.Id);
            Uri uri = new Uri(putRequest);

            try
            {
                string json = JsonConvert.SerializeObject(item);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;

                response = await client.PutAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"\tFluctuantVariable successfully saved.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
                return false;
            }

            return true;
        }
    }
}
