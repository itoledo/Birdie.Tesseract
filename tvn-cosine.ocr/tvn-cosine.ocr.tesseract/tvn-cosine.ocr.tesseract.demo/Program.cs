using System;
using Tesseract;

namespace TestConsole
{
    class Program
    {
        private const string tessdata = @"C:/Program Files (x86)/Tesseract-OCR/tessdata/";
        private const string language = "eng";
        private const string image = @"Test.png";

        static void Main(string[] args)
        { 
            string language = @"eng";
            using (var api = new TessBaseAPI(tessdata, language))
            {
                api.Process(image);
                string text = api.GetUTF8Text();
            }

            Console.WriteLine("Done...");
            Console.ReadKey();
        }
    }
}
