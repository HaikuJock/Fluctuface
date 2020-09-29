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

        public List<FluctuantVariable> Items { get; private set; }

        public RestService()
        {
#if DEBUG
            client = new HttpClient(DependencyService.Get<IHttpClientHandlerService>().GetInsecureHandler());
#else
            client = new HttpClient();
#endif
        }

        public async Task<IEnumerable<FluctuantVariable>> RefreshDataAsync()
        {
            Items = new List<FluctuantVariable>();

            Uri uri = new Uri(string.Format(Constants.GetRestUrl, string.Empty));
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
            Uri uri = new Uri(string.Format(Constants.PutRestUrl, item.Id));

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
