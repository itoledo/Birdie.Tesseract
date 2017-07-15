using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tvn.cosine.ai.logic.propositional.kb;

namespace tvn_cosine.ai.test.logic.propositional.kb
{
    [TestClass]
    public class KnowledgeBaseTest
    {
        private KnowledgeBase kb;

        [TestInitialize]
        public void setUp()
        {
            kb = new KnowledgeBase();
        }

        [TestMethod]
        public void testTellInsertsSentence()
        {
            kb.tell("(A & B)");
            Assert.AreEqual(1, kb.size());
        }

        [TestMethod]
        public void testTellDoesNotInsertSameSentenceTwice()
        {
            kb.tell("(A & B)");
            Assert.AreEqual(1, kb.size());
            kb.tell("(A & B)");
            Assert.AreEqual(1, kb.size());
        }

        [TestMethod]
        public void testEmptyKnowledgeBaseIsAnEmptyString()
        {
            Assert.AreEqual("", kb.ToString());
        }

        [TestMethod]
        public void testKnowledgeBaseWithOneSentenceToString()
        {
            kb.tell("(A & B)");
            Assert.AreEqual("A & B", kb.ToString());
        }

        [TestMethod]
        public void testKnowledgeBaseWithTwoSentencesToString()
        {
            kb.tell("(A & B)");
            kb.tell("(C & D)");
            Assert.AreEqual("A & B & C & D", kb.ToString());
        }

        [TestMethod]
        public void testKnowledgeBaseWithThreeSentencesToString()
        {
            kb.tell("(A & B)");
            kb.tell("(C & D)");
            kb.tell("(E & F)");
            Assert.AreEqual(
                    "A & B & C & D & E & F",
                    kb.ToString());
        }
    }
}
