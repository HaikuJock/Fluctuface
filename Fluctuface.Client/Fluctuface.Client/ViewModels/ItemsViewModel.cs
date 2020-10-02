using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using Fluctuface.Client.Models;
using Fluctuface.Client.Views;

namespace Fluctuface.Client.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableCollection<UpdatingFluctuant> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public ItemsViewModel()
        {
            Title = "Browse";
            Items = new ObservableCollection<UpdatingFluctuant>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            //MessagingCenter.Subscribe<NewItemPage, UpdatingFluctuant>(this, "AddItem", async (obj, item) =>
            //{
            //    var newItem = item as UpdatingFluctuant;
            //    Items.Add(newItem);
            //    await DataStore.AddItemAsync(newItem);
            //});
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(new UpdatingFluctuant(DataStore, item));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}