using System;
namespace Fluctuface.Client
{
    public static class Constants
    {
        // URL of REST service
        //public static string RestUrl = Device.RuntimePlatform == Device.Android ? "https://10.0.2.2:5001/api/todoitems/{0}" : "https://localhost:5001/api/todoitems/{0}";
        public static string GetRestUrl = "http://192.168.0.2:52596/api/FluctuantVariables";
        public static string PutRestUrl = "http://192.168.0.2:52596/api/FluctuantVariables/{0}";
    }
}
