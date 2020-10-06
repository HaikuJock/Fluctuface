using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Haiku.Fluctuface.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseUrls("http://*:" + Constants.ServicePort.ToString())
                        .UseStartup<Startup>();
                });
    }
}
