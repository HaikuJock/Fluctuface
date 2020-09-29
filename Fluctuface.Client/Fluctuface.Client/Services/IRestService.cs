using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Fluctuface.Client.Services
{
	public interface IRestService
	{
		Task<List<FluctuantVariable>> RefreshDataAsync();

		Task SaveAsync(FluctuantVariable item);
	}
}
