using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.ai.common.exceptions;
using tvn.cosine.ai.nlp.parsing.grammars;

namespace tvn_cosine.ai.test.unit.nlp.parse
{
    [TestClass]
    public class RuleTest
    {
        Rule testR;

        [TestMethod]
        public void testStringSplitConstructor()
        {
            testR = new Rule("A,B", "a,bb,c", (float)0.50);
            Assert.AreEqual(testR.lhs.Size(), 2);
            Assert.AreEqual(testR.rhs.Size(), 3);
            Assert.AreEqual(testR.lhs.Get(1), "B");
            Assert.AreEqual(testR.rhs.Get(2), "c");
        }

        [TestMethod]
        public void testStringSplitConstructorOnEmptyStrings()
        {
            testR = new Rule("", "", (float)0.50);
            Assert.AreEqual(testR.lhs.Size(), 0);
            Assert.AreEqual(testR.rhs.Size(), 0);
        }

        [TestMethod]
        public void testStringSplitConstructorOnCommas()
        {
            testR = new Rule(",", ",", (float)0.50);
            Assert.AreEqual(testR.lhs.Size(), 0);
            Assert.AreEqual(testR.rhs.Size(), 0);
        }


        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void testStringSplitConstructorElementAccess()
        {
            testR = new Rule(",", "", (float)0.50);
            testR.lhs.Get(0);
        }
    }
}
