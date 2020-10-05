using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Fluctuface.Client.Services;
using Xamarin.Forms;

namespace Fluctuface.Client.ViewModels
{
    public class UpdatingFluctuant : FluctuantVariable, INotifyPropertyChanged
    {
        IDataStore<FluctuantVariable> dataStore;

        public float SliderValue // 0...1
        {
            get
            {
                return (Value - Min) / (Max - Min);
            }
            set
            {
                var old = Value;
                Value = Min + value * (Max - Min);
                if (old != Value)
                {
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public UpdatingFluctuant(IDataStore<FluctuantVariable> dataStore, FluctuantVariable clone)
            : base(clone.Id, clone.Min, clone.Max, clone.Value)
        {
            this.dataStore = dataStore;
        }

        void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            Debug.WriteLine($"Onchanged Value to {Value}");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
            dataStore.UpdateItemAsync(this);
        }
    }
}
