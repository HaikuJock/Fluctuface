using System;
using System.Threading.Tasks;
using System.Collections.Generic;

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

		//public Task<List<FluctuantVariable>> GetTasksAsync()
		//{
		//	return restService.RefreshDataAsync();
		//}

		//public Task SaveTaskAsync(FluctuantVariable item)
		//{
		//	return restService.SaveAsync(item);
		//}
	}
}
