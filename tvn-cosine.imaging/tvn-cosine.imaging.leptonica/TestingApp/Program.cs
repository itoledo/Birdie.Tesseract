using Leptonica;
using System; 
using Tvn.Cosine.Imaging;

namespace TestingApp
{
    class Program
    {
        static string file = @"C:\Temp\1.jpg";

        static void Main(string[] args)
        {
            var pix = Pix.Read(file);
            var pixWithBorder = Pix2.pixAddBorder(pix, 23, Color.Red);
            pixWithBorder.Write(@"C:\Temp\pixWithBorder.jpg");

            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
