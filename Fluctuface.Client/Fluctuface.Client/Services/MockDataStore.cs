using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fluctuface.Client.Services
{
    public class MockDataStore : IDataStore<FluctuantVariable>
    {
        readonly List<FluctuantVariable> items;

        public MockDataStore()
        {
            items = new List<FluctuantVariable>()
            {
                new FluctuantVariable { Id = "First item",  Value=0.5f },
                new FluctuantVariable { Id = "Second item", Value=0.5f },
                new FluctuantVariable { Id = "Third item",  Value=0.5f },
                new FluctuantVariable { Id = "Fourth item", Value=0.5f },
                new FluctuantVariable { Id = "Fifth item",  Value=0.5f },
                new FluctuantVariable { Id = "Sixth item",  Value=0.5f }
            };
        }

        //public async Task<bool> AddItemAsync(FluctuantVariable item)
        //{
        //    items.Add(item);

        //    return await Task.FromResult(true);
        //}

        public async Task<bool> UpdateItemAsync(FluctuantVariable item)
        {
            var oldItem = items.Where((FluctuantVariable arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        //public async Task<bool> DeleteItemAsync(string id)
        //{
        //    var oldItem = items.Where((FluctuantVariable arg) => arg.Id == id).FirstOrDefault();
        //    items.Remove(oldItem);

        //    return await Task.FromResult(true);
        //}

        //public async Task<FluctuantVariable> GetItemAsync(string id)
        //{
        //    return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        //}

        public async Task<IEnumerable<FluctuantVariable>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}
