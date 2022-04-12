//Author: Dominik Dohmeier
using System;
using System.Diagnostics;

namespace TestingPlayGround
{
    internal class Program
    {
        private const int COLOR_COUNT = 2;
        private const int DIMENSIONS = 2;
        private const int MAX_REPITITIONS = 100000000;

        private static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();

            int[,] colorCounts = new int[COLOR_COUNT, DIMENSIONS];

            stopwatch.Start();

            for (int repititions = 0; repititions < MAX_REPITITIONS; repititions++)
            {
                for (int i = 0; i < COLOR_COUNT; i++)
                {
                    for (int j = 0; j < DIMENSIONS; j++)
                    {
                        colorCounts[i, j] = 0;
                    }
                }
            }

            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed.TotalMilliseconds +"ms");

            stopwatch.Reset();

            stopwatch.Start();

            for (int repititions = 0; repititions < MAX_REPITITIONS; repititions++)
            {
                colorCounts = new int[COLOR_COUNT, DIMENSIONS];
            }

            stopwatch.Stop();

            Console.WriteLine(stopwatch.Elapsed.TotalMilliseconds + "ms");
            Console.ReadLine();
        }
    }
}