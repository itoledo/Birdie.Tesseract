using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.collections;
using tvn.cosine.collections.api;
using tvn.cosine.ai.logic.propositional.parsing.ast;

namespace tvn_cosine.ai.test.unit.logic.propositional.parsing
{
    [TestClass] public class ListTest
    {

        [TestMethod]
        public void testListOfSymbolsClone()
        {
          ICollection<PropositionSymbol> l = CollectionFactory.CreateQueue<PropositionSymbol>();
            l.Add(new PropositionSymbol("A"));
            l.Add(new PropositionSymbol("B"));
            l.Add(new PropositionSymbol("C"));
         ICollection<PropositionSymbol> l2 = CollectionFactory.CreateQueue<PropositionSymbol>(l);
            l2.Remove(new PropositionSymbol("B"));
            Assert.AreEqual(3, l.Size());
            Assert.AreEqual(2, l2.Size());
        }

        [TestMethod]
        public void testListRemove()
        {
         ICollection<int> one = CollectionFactory.CreateQueue<int>();
            one.Add(1);
            Assert.AreEqual(1, one.Size());
            one.RemoveAt(0);
            Assert.AreEqual(0, one.Size());
        }
    }

}
