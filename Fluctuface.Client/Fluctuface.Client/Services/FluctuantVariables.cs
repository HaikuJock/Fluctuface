using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Fluctuface.Client.Services
{
    public class FluctuantVariables
    {
		IRestService restService;

		public FluctuantVariables(IRestService service)
		{
			restService = service;
		}

		public Task<List<FluctuantVariable>> GetTasksAsync()
		{
			return restService.RefreshDataAsync();
		}

		public Task SaveTaskAsync(FluctuantVariable item)
		{
			return restService.SaveAsync(item);
		}
	}
}
