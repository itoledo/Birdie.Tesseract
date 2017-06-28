using Leptonica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
    class Program
    {
        private const string tessdata = @"C:/Program Files (x86)/Tesseract-OCR/tessdata/";
        private const string language = "eng";
        private const string image = @"C:\Temp\5.jpg";

        static void Main(string[] args)
        {
            Tesseract.TessBaseAPI api = new Tesseract.TessBaseAPI(tessdata, language);
            var pix = Pix.Read(image);
            //api.SetInputImage(pix);
            //var outText = api.GetUTF8Text();
            //api.End(); 

            Console.WriteLine("Done...");
            Console.ReadKey();
        }
    }
}
