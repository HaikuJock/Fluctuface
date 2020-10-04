using System;
using System.Threading;
using System.Threading.Tasks;

namespace Fluctuface.SampleApp
{
    class App
    {
        [Fluctuant("FirstFluctuantFloat", 0f, 1f)]
        public static float FluctuatingFloat = 0.5f;
        static float PreviousFluctuating = 0.5f;

        [Fluctuant("SecondFluctuantFloat", 10f, 90f)]
        public static float SecondFluctuatingFloat = 60.0f;
        static float SecondPreviousFluctuating = 60.0f;

        static FluctuantPatron patron;

        static void Main(string[] args)
        {
            Console.WriteLine("Fluctuface SampleApp!");
            patron = new FluctuantPatron();
            patron.ExposeFluctuants();
            Task.Factory.StartNew(ReportFloatChanges);
            Console.ReadLine();
        }

        static void ReportFloatChanges()
        {
            while (true)
            {
                if (PreviousFluctuating != FluctuatingFloat)
                {
                    Console.WriteLine($"1st: Change from: {PreviousFluctuating} to: {FluctuatingFloat}");
                    PreviousFluctuating = FluctuatingFloat;
                }
                if (SecondPreviousFluctuating != SecondFluctuatingFloat)
                {
                    Console.WriteLine($"2nd: Change from: {SecondPreviousFluctuating} to: {SecondFluctuatingFloat}");
                    SecondPreviousFluctuating = SecondFluctuatingFloat;
                }
                Thread.Sleep(1000);
            }
        }
    }
}
