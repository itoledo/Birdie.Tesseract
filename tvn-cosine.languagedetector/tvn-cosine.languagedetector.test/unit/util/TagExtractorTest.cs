using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn_cosine.languagedetector.util;

namespace tvn_cosine.languagedetector.test.unit.util
{
    [TestClass]
    public class TagExtractorTest
    { 
        [TestMethod]
        public void testTagExtractor()
        {
            TagExtractor extractor = new TagExtractor(null, 0);
            Assert.AreEqual(extractor.target_, null);
            Assert.AreEqual(extractor.threshold_, 0);

            TagExtractor extractor2 = new TagExtractor("abstract", 10);
            Assert.AreEqual(extractor2.target_, "abstract");
            Assert.AreEqual(extractor2.threshold_, 10);
        }
         
        [TestMethod]
        public void testSetTag()
        {
            TagExtractor extractor = new TagExtractor(null, 0);
            extractor.setTag("");
            Assert.AreEqual(extractor.tag_, "");
            extractor.setTag(null);
            Assert.AreEqual(extractor.tag_, null);
        }
         
        [TestMethod]
        public void testAdd()
        {
            TagExtractor extractor = new TagExtractor(null, 0);
            extractor.add("");
            extractor.add(null);    // ignore
        }
         
        [TestMethod]
        public void testCloseTag()
        {
            TagExtractor extractor = new TagExtractor(null, 0);
            extractor.closeTag();    // ignore
        }
         
        [TestMethod]
        public void testNormalScenario()
        {
            TagExtractor extractor = new TagExtractor("abstract", 10);
            Assert.AreEqual(extractor.count(), 0);

            LangProfile profile = new LangProfile("en");

            // normal
            extractor.setTag("abstract");
            extractor.add("This is a sample text.");
            profile.update(extractor.closeTag());
            Assert.AreEqual(extractor.count(), 1);
            Assert.AreEqual(profile.n_words[0], 17);  // Thisisasampletext
            Assert.AreEqual(profile.n_words[1], 22);  // _T, Th, hi, ...
            Assert.AreEqual(profile.n_words[2], 17);  // _Th, Thi, his, ...

            // too short
            extractor.setTag("abstract");
            extractor.add("sample");
            profile.update(extractor.closeTag());
            Assert.AreEqual(extractor.count(), 1);

            // other tags
            extractor.setTag("div");
            extractor.add("This is a sample text which is enough long.");
            profile.update(extractor.closeTag());
            Assert.AreEqual(extractor.count(), 1);
        }
         
        [TestMethod]
        public void testClear()
        {
            TagExtractor extractor = new TagExtractor("abstract", 10);
            extractor.setTag("abstract");
            extractor.add("This is a sample text.");
            Assert.AreEqual(extractor.buf_.ToString(), "This is a sample text.");
            Assert.AreEqual(extractor.tag_, "abstract");
            extractor.clear();
            Assert.AreEqual(extractor.buf_.ToString(), "");
            Assert.AreEqual(extractor.tag_, null);
        } 
    } 
}
