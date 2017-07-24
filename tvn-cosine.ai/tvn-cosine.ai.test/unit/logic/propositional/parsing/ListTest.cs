using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.logic.propositional.parsing.ast;

namespace tvn_cosine.ai.test.unit.logic.propositional.parsing
{
    [TestClass] public class ListTest
    {

        [TestMethod]
        public void testListOfSymbolsClone()
        {
          IQueue<PropositionSymbol> l = Factory.CreateQueue<PropositionSymbol>();
            l.Add(new PropositionSymbol("A"));
            l.Add(new PropositionSymbol("B"));
            l.Add(new PropositionSymbol("C"));
         IQueue<PropositionSymbol> l2 = Factory.CreateQueue<PropositionSymbol>(l);
            l2.Remove(new PropositionSymbol("B"));
            Assert.AreEqual(3, l.Size());
            Assert.AreEqual(2, l2.Size());
        }

        [TestMethod]
        public void testListRemove()
        {
         IQueue<int> one = Factory.CreateQueue<int>();
            one.Add(1);
            Assert.AreEqual(1, one.Size());
            one.RemoveAt(0);
            Assert.AreEqual(0, one.Size());
        }
    }

}
