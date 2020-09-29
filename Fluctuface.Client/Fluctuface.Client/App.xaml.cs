using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Fluctuface.Client.Services;
using Fluctuface.Client.Views;

namespace Fluctuface.Client
{
    public partial class App : Application
    {
        public static FluctuantVariables Variables { get; private set; }

        public App()
        {
            InitializeComponent();

            //Variables = new FluctuantVariables(new RestService());

            DependencyService.Register<FluctuantVariables>();
            //DependencyService.Register<MockDataStore>();
            MainPage = new MainPage();
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
