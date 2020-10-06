using Haiku.Fluctuface.Client.Services;
using Haiku.Fluctuface.Client.Views;
using Xamarin.Forms;

namespace Haiku.Fluctuface.Client
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
