using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Haiku.Fluctuface.Client.Services
{
    public interface IDataStore<T>
    {
        Task<bool> UpdateItemAsync(T item);
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);
    }
}
