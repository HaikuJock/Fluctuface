using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Fluctuface.Server
{
    public class Program
    {
        [Fluctuant("First Fluctuant Float", 0f, 1f)]
        public static float FluctuatingFloat = 0.5f;
        static float PreviousFluctuating = 0f;

        public static void Main(string[] args)
        {
            Task.Factory.StartNew(ReportFloatChanges);
            CreateHostBuilder(args).Build().Run();
        }

        static void ReportFloatChanges()
        {
            while (true)
            {
                if (PreviousFluctuating != FluctuatingFloat)
                {
                    Console.WriteLine($"Change from: {PreviousFluctuating} to: {FluctuatingFloat}");
                    PreviousFluctuating = FluctuatingFloat;
                }
                Thread.Sleep(1000);
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
