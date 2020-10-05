using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Fluctuface.Client.Services;
using Fluctuface.Client.Views;

namespace Fluctuface.Client
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            DependencyService.Register<FluctuantVariables>();
            //DependencyService.Register<MockDataStore>();
            MainPage = new ItemsPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
