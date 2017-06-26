using System;
using System.Diagnostics;
using tvn.cosine.ai.Datastructures;

namespace TvnTestConsoleApp
{
    class Program
    {
        const int iterations = 1000000;
        //var d1 = new long[] { 1L, 1L, 1L, 1L, 1L, 1L, 1L, 1L, 1L, 1L, 1L, 1L, 1L, 1L, 56L, 56L, 56L, 56L, 56L, 56L, 56L, 56L, 56L, 56L, 56L, 56L, 56L, 56L, 56L, 56L, 56L, 56L, 56L, 56L, 56L, 56L, 56L, 56L, 56L, 56L, 56L, 56L, 56L, 56L, 56L, 56L, 56L, 56L, 56L, 56L, 56L, 56L, 56L, 56L, 56L, 56L, 56L, 56L, 56L, 56L, 33L, };
        //var d2 = new long[] { 0L, 0L, 1L, 1L, 1L, 1L, 1L, 1L, 1L, 1L, 1L, 1L, 1L, 1L, 33L, 33L, 33L, 33L, 33L, 33L, 33L, 33L, 33L, 33L, 33L, 33L, 33L, 33L, 33L, 33L, 33L, 33L, 33L, 33L, 33L, 33L, 33L, 33L, 33L, 33L, 33L, 33L, 33L, 33L, 33L, 33L, 33L, 33L, 33L, 33L, 33L, 33L, 33L, 33L, 33L, 33L, 33L, 33L, 33L, 33L, 33L, };


        static void Main(string[] args)
        {
            var testing = new long[] { 3L, 3L, 4L, 56L, 56L, 56L };

            TPlot plot = new TPlot(testing.Length);
            plot.Add(new long[] { 2L, 1L, 56L, 1L, 56L, 56L });
            plot.Add(new long[] { 2L, 3L, 56L, 3L, 56L, 56L });
            plot.Add(new long[] { 4L, 3L, 56L, 3L, 56L, 56L });
            plot.Add(new long[] { 4L, 1L, 56L, 1L, 56L, 56L });

            var answer = TPlot.GetEuclideanDistance(testing, plot[0]);
            var centre = plot.CalculateCentrePoint();
            var distance = plot.GetEuclideanDistanceFromCentre(testing);
            var d = TPlot.GetAngle(new long[] { 0, 3 }, new long[] { 4, 0 });

            Console.WriteLine("Normal Euclidean distance: {0}", answer);


            Console.WriteLine("Complete, press <ENTER> to quit");
            Console.ReadLine();
        }

    }
}
