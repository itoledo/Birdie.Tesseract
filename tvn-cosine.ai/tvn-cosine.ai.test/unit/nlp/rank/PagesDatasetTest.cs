using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.nlp.ranking;

namespace tvn_cosine.ai.test.unit.nlp.rank
{
    [TestClass]
    public class PagesDatasetTest
    {

        //IMap<string, Page> pageTable;
        //// resource folder of .txt files to test with
        //string testFilesFolderPath = "src/main/resources/aima/core/ranking/data/pages/test_pages";

        [TestMethod]
        public void testGetPageName()
        {
            FileInfo file = new FileInfo("test/file/path.txt");
            FileInfo fileTwo = new FileInfo("test/file/PATHTWO.txt");
            string p = PagesDataset.getPageName(file);
            Assert.AreEqual(p, "/wiki/path");
            Assert.AreEqual(PagesDataset.getPageName(fileTwo), "/wiki/pathtwo");
        }


        ////[Ignore]//("testFilesFolderPath currently breaks portability") 
        ////[TestMethod]
        ////public void testLoadPages()
        ////{
        ////    string folderPath = testFilesFolderPath;
        ////    pageTable = PagesDataset.loadPages(folderPath);
        ////    Assert.IsTrue(pageTable.Size() > 0);
        ////    Assert.IsTrue(pageTable.ContainsKey("/wiki/TestMan".ToLower()));
        ////}


    ////    [Ignore]//("testFilesFolderPath currently breaks portability")
    
    ////[TestMethod]
    ////    public void testLoadPagesInlinks()
    ////    {
    ////        string folderPath = testFilesFolderPath;
    ////        pageTable = PagesDataset.loadPages(folderPath);
    ////        // TestMan.txt should have the following inlinks
    ////        // "/wiki/testdog", "/wiki/testgorilla", "/wiki/testliving", "/wiki/testturnerandhooch"
    ////        Page testPage = pageTable.Get("/wiki/testman");
    ////        Assert.IsTrue(testPage.getInlinks().ContainsAll(Factory.CreateQueue<string>(new[] { "/wiki/testdog",
    ////                                                                    "/wiki/testgorilla",
    ////                                                                    "/wiki/testliving",
    ////                                                                    "/wiki/testturnerandhooch"})));
    ////    }


    ////    [Ignore] //("testFilesFolderPath currently breaks portability")

    ////    [TestMethod]
    ////    public void testLoadFileText()
    ////    {
    ////        string testFilePath = "TestMan.txt";
    ////        FileInfo folder = new FileInfo(testFilesFolderPath);
    ////        FileInfo f = new FileInfo(testFilePath);
    ////        string content = PagesDataset.loadFileText(testFilesFolderPath, f);
    ////        Assert.AreNotEqual(content, null);
    ////        Assert.AreNotEqual(content, "");
    ////        Assert.IsTrue(content.Contains("Keyword string 1: A man is a male human."));

    ////    }
    }

}
