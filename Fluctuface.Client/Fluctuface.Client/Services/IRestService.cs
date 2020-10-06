using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Haiku.Fluctuface.Client.Services
{
	public interface IRestService
	{
		Task<IEnumerable<FluctuantVariable>> RefreshDataAsync();

		Task<bool> SaveAsync(FluctuantVariable item);
	}
}
