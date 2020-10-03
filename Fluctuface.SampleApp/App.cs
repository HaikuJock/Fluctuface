using System;
using System.Threading;
using System.Threading.Tasks;

namespace Fluctuface.SampleApp
{
    class App
    {
        [Fluctuant("First Fluctuant Float", 0f, 1f)]
        public static float FluctuatingFloat = 0.5f;
        static float PreviousFluctuating = 0f;

        static FluctuantPatron patron;

        static void Main(string[] args)
        {
            Console.WriteLine("Fluctuface SampleApp!");
            patron = new FluctuantPatron();
            patron.ExposeFluctuants();
            Task.Factory.StartNew(ReportFloatChanges);
            Console.ReadLine();
            patron.OnExit();
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
    }
}
