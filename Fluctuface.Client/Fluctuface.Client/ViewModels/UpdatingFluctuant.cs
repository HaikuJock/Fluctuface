using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Fluctuface.Client.Services;
using Xamarin.Forms;

namespace Fluctuface.Client.ViewModels
{
    public class UpdatingFluctuant : FluctuantVariable
    {
        IDataStore<FluctuantVariable> dataStore;

        public float SliderValue
        {
            get => Value;
            set
            {
                Value = value;
                OnPropertyChanged();
            }
        }

        void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            Console.WriteLine($"Onchanged {propertyName} to {Value}");
            dataStore.UpdateItemAsync(this);
        }

        public UpdatingFluctuant(IDataStore<FluctuantVariable> dataStore, FluctuantVariable clone)
            : base(clone.Id, clone.Name, clone.Min, clone.Max, clone.Value)
        {
            this.dataStore = dataStore;
        }
    }
}
