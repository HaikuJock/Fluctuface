using System;

using Fluctuface.Client.Models;

namespace Fluctuface.Client.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public FluctuantVariable Item { get; set; }
        public ItemDetailViewModel(FluctuantVariable item = null)
        {
            Title = item?.Id;
            Item = item;
        }
    }
}
