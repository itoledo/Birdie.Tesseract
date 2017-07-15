using Microsoft.VisualStudio.TestTools.UnitTesting;
using System; 
using tvn.cosine.ai.logic.propositional.parsing.ast;

namespace tvn_cosine.ai.test.logic.propositional.parsing
{
    [TestClass]
    public class ComplexSentenceTest
    {

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void test_IllegalArgumentOnConstruction_1()
        {
            new ComplexSentence(null, new Sentence[] { new PropositionSymbol("A"), new PropositionSymbol("B") });
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void test_IllegalArgumentOnConstruction_2()
        {
            new ComplexSentence(Connective.NOT, (Sentence[])null);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void test_IllegalArgumentOnConstruction_3()
        {
            new ComplexSentence(Connective.NOT, new Sentence[] { });
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void test_IllegalArgumentOnConstruction_4()
        {
            new ComplexSentence(Connective.NOT, new Sentence[] { new PropositionSymbol("A"), new PropositionSymbol("B") });
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void test_IllegalArgumentOnConstruction_5()
        {
            new ComplexSentence(Connective.AND, new Sentence[] { new PropositionSymbol("A") });
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void test_IllegalArgumentOnConstruction_6()
        {
            new ComplexSentence(Connective.AND, new Sentence[] { new PropositionSymbol("A"), new PropositionSymbol("B"), new PropositionSymbol("C") });
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void test_IllegalArgumentOnConstruction_7()
        {
            new ComplexSentence(Connective.OR, new Sentence[] { new PropositionSymbol("A") });
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void test_IllegalArgumentOnConstruction_8()
        {
            new ComplexSentence(Connective.OR, new Sentence[] { new PropositionSymbol("A"), new PropositionSymbol("B"), new PropositionSymbol("C") });
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void test_IllegalArgumentOnConstruction_9()
        {
            new ComplexSentence(Connective.IMPLICATION, new Sentence[] { new PropositionSymbol("A") });
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void test_IllegalArgumentOnConstruction_10()
        {
            new ComplexSentence(Connective.IMPLICATION, new Sentence[] { new PropositionSymbol("A"), new PropositionSymbol("B"), new PropositionSymbol("C") });
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void test_IllegalArgumentOnConstruction_11()
        {
            new ComplexSentence(Connective.BICONDITIONAL, new Sentence[] { new PropositionSymbol("A") });
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void test_IllegalArgumentOnConstruction_12()
        {
            new ComplexSentence(Connective.BICONDITIONAL, new Sentence[] { new PropositionSymbol("A"), new PropositionSymbol("B"), new PropositionSymbol("C") });
        }
    }
}
