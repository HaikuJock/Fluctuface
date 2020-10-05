using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fluctuface.Client.Services
{
    public class FluctuantVariables : IDataStore<FluctuantVariable>
	{
		IRestService restService;

		public Task<IEnumerable<FluctuantVariable>> GetItemsAsync(bool forceRefresh = false)
        {
			return restService.RefreshDataAsync();
		}

		public Task<bool> UpdateItemAsync(FluctuantVariable item)
        {
			return restService.SaveAsync(item);
		}

		public FluctuantVariables()
			: this(new RestService())
        {
        }

		public FluctuantVariables(IRestService service)
		{
			restService = service;
		}
	}
}
