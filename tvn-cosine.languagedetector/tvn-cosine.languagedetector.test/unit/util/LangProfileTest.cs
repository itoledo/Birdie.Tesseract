using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tvn_cosine.languagedetector.util;

namespace tvn_cosine.languagedetector.test.unit.util
{
    [TestClass]
    public class LangProfileTest
    {
        [TestMethod]
        public void testLangProfile()
        {
            LangProfile profile = new LangProfile();
            Assert.AreEqual(profile.name, null);
        }

        [TestMethod]
        public void testLangProfileStringInt()
        {
            LangProfile profile = new LangProfile("en");
            Assert.AreEqual(profile.name, "en");
        }

        [TestMethod]
        public void testAdd()
        {
            LangProfile profile = new LangProfile("en");
            profile.add("a");
            Assert.AreEqual((int)profile.freq["a"], 1);
            profile.add("a");
            Assert.AreEqual((int)profile.freq["a"], 2);
            profile.omitLessFreq();
        }


        [TestMethod]
        public void testAddIllegally1()
        {
            LangProfile profile = new LangProfile(); // Illegal ( available for only JSONIC ) but ignore  
            profile.add("a"); // ignore
            Assert.IsFalse(profile.freq.ContainsKey("a")); // ignored
        }

        [TestMethod]
        public void testAddIllegally2()
        {
            LangProfile profile = new LangProfile("en");
            profile.add("a");
            profile.add("");  // Illegal (string's length of parameter must be between 1 and 3) but ignore
            profile.add("abcd");  // as well
            Assert.AreEqual((int)profile.freq["a"], 1);
            Assert.IsFalse(profile.freq.ContainsKey(""), null);     // ignored
            Assert.IsFalse(profile.freq.ContainsKey("abcd"), null); // ignored

        }

        [TestMethod]
        public void testOmitLessFreq()
        {
            LangProfile profile = new LangProfile("en");
            string[] grams = "a b c \u3042 \u3044 \u3046 \u3048 \u304a \u304b \u304c \u304d \u304e \u304f".Split(' ');
            for (int i = 0; i < 5; ++i)
            {
                foreach (string g in grams)
                {
                    profile.add(g);
                }
            }
            profile.add("\u3050");

            Assert.AreEqual((int)profile.freq["a"], 5);
            Assert.AreEqual((int)profile.freq["\u3042"], 5);
            Assert.AreEqual((int)profile.freq["\u3050"], 1);
            profile.omitLessFreq();
            Assert.IsFalse(profile.freq.ContainsKey("a")); // omitted
            Assert.AreEqual((int)profile.freq["\u3042"], 5);
            Assert.IsFalse(profile.freq.ContainsKey("\u3050")); // omitted
        }

        [TestMethod]
        public void testOmitLessFreqIllegally()
        {
            LangProfile profile = new LangProfile();
            profile.omitLessFreq();  // ignore
        }
    }
}
