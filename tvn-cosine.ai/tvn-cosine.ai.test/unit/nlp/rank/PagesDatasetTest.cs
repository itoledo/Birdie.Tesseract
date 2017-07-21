using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO; 
using tvn.cosine.ai.nlp.ranking;

namespace tvn_cosine.ai.test.unit.nlp.rank
{
    [TestClass]
    public class PagesDatasetTest
    { 
        [TestMethod]
        public void testGetPageName()
        {
            FileInfo file = new FileInfo("test/file/path.txt");
            FileInfo fileTwo = new FileInfo("test/file/PATHTWO.txt");
            string p = PagesDataset.getPageName(file);
            Assert.AreEqual(p, "/wiki/path");
            Assert.AreEqual(PagesDataset.getPageName(fileTwo), "/wiki/pathtwo");
        } 
    }

}
