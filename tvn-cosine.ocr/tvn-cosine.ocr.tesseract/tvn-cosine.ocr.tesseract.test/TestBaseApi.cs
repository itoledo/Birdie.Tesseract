using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tesseract;

namespace tvn_cosine.ocr.tesseract.test
{
    [TestClass]
    public class TestBaseApi
    {
        private const string image = @"Test.png";
        private const string tessdata = @"C:/Program Files (x86)/Tesseract-OCR/tessdata/";
        private const string language = "eng";
        public TessBaseAPI api;

        [TestInitialize]
        public void TestInitialise()
        {
            api = new TessBaseAPI(tessdata, language);
        }

        [TestCleanup]
        public void TestDispose()
        {
            api.Dispose();
        }

        [TestMethod]
        public void TestGetUTF8Text()
        {
            api.Process(image);
            string text = api.GetUTF8Text();

            Assert.IsTrue(text.Contains("Canada, Georgia, Ireland, Mauritius and"));
            Assert.IsTrue(text.Contains("Of course, compared to our Brics peers,"));
            Assert.IsTrue(text.Contains("Congo, Libya and Venezuela at 159th"));
            Assert.IsTrue(text.Contains("However, SA has lost more ground than"));
            Assert.IsTrue(text.Contains("China has shed 13 places, and Russia has"));
            Assert.IsTrue(text.Contains("tralia and the UK tied for 10th place"));
        }
    }
}
