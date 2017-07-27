using Leptonica;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace TestConsole
{
    class Program
    {
        private const string tessdata = @"C:/Program Files (x86)/Tesseract-OCR/tessdata/";
        private const string language = "eng";
        private const string image = @"C:\Temp\5.jpg";

        static void Main(string[] args)
        { 
            string language = @"eng";
            using (var api = new TessBaseAPI(tessdata, language))
            {
                api.Process(image, true);
                string text = api.GetUTF8Text();
            }

            Console.WriteLine("Done...");
            Console.ReadKey();
        }
    }
}
