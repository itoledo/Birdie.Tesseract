using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tvn.cosine.ai.logic.propositional.parsing.ast;

namespace tvn_cosine.ai.test.logic.propositional.parsing
{
    [TestClass]
    public class ListTest
    {

        [TestMethod]
     public void testListOfSymbolsClone()
        {
            List<PropositionSymbol> l = new List<PropositionSymbol>();
            l.Add(new PropositionSymbol("A"));
            l.Add(new PropositionSymbol("B"));
            l.Add(new PropositionSymbol("C"));
            List<PropositionSymbol> l2 = new List<PropositionSymbol>(l);
            l2.Remove(new PropositionSymbol("B"));
            Assert.AreEqual(3, l.Count);
            Assert.AreEqual(2, l2.Count);
        }

        [TestMethod]
     public void testListRemove()
        {
            List<int> one = new List<int>();
            one.Add(1);
            Assert.AreEqual(1, one.Count);
            one.Remove(0);
            Assert.AreEqual(0, one.Count);
        }
    }
}
