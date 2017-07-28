using Microsoft.VisualStudio.TestTools.UnitTesting; 

namespace tvn_cosine.languagedetector.test.unit
{
    [TestClass]
    public class LanguageTest
    { 
        [TestMethod]
        public void testLanguage()
        {
            Language lang = new Language(null, 0);
            Assert.AreEqual(lang.lang, null);
            Assert.AreEqual(lang.prob, 0.0, 0.0001);
            Assert.AreEqual(lang.ToString(), "");

            Language lang2 = new Language("en", 1.0);
            Assert.AreEqual(lang2.lang, "en");
            Assert.AreEqual(lang2.prob, 1.0, 0.0001);
            Assert.AreEqual(lang2.ToString(), "en:1,0"); 
        } 
    } 
}
