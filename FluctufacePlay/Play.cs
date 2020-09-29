using System;
using Fluctuface;

namespace FluctufacePlay
{
    class Play
    {
        [Fluctuant("First Fluctuant Float", 0f, 1f)]
        public static float FluctuatingFloat = 0.5f;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }
    }
}
