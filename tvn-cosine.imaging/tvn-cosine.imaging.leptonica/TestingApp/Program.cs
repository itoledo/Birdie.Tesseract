using Leptonica;
using System;

namespace TestingApp
{
    class Program
    {
        static string file = @"C:\Temp\1.jpg";
        static string output = @"C:\Temp\1Output.jpg";

        static void Main(string[] args)
        {
            Pix pix = Pix.Read(file);
            var newPix = pix.pixCleanBackgroundToWhite(null, null);
            newPix.Write(output, ImageFileFormatTypes.IFF_BMP);

            Console.WriteLine("Done...");
            Console.ReadKey();
        }
    }
}
