using Microsoft.VisualStudio.TestTools.UnitTesting;
using Leptonica;

namespace tvn_cosine.imaging.leptonica.test
{
    [TestClass]
    public class TestPix
    {
        private const string file = "Test.png";

        [TestMethod]
        public void TestPixRead()
        {
            Pix pix = Pix.Read(file);

            Assert.AreEqual(1421, pix.Width);
            Assert.AreEqual(1949, pix.Height);
            Assert.AreEqual(ImageFileFormatTypes.IFF_PNG, pix.InputFormat);
            Assert.AreEqual(96, pix.XRes);
            Assert.AreEqual(96, pix.YRes);
        }
    }
}
