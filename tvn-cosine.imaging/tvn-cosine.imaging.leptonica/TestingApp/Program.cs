using Leptonica;
using System;

namespace TestingApp
{
    class Program
    {
        static string file = @"C:\Temp\1.jpg";

        static void Main(string[] args)
        {
            Pix pix = Pix.Read(file);

            Console.WriteLine("Done...");
            Console.ReadKey();
        }
    }
}
