using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.logic.fol.kb.data;
using tvn.cosine.ai.logic.fol.parsing.ast;

namespace tvn_cosine.ai.test.unit.logic.fol.kb.data
{
    [TestClass]
    public class ChainTest
    {
        [TestMethod]
        public void testIsEmpty()
        {
            Chain c = new Chain();

            Assert.IsTrue(c.isEmpty());

            c.addLiteral(new Literal(new Predicate("P", Factory.CreateQueue<Term>())));

            Assert.IsFalse(c.isEmpty());

            IQueue<Literal> lits = Factory.CreateQueue<Literal>();

            lits.Add(new Literal(new Predicate("P", Factory.CreateQueue<Term>())));

            c = new Chain(lits);

            Assert.IsFalse(c.isEmpty());
        }

        [TestMethod]
        public void testContrapositives()
        {
            IQueue<Chain> conts;
            Literal p = new Literal(new Predicate("P", Factory.CreateQueue<Term>()));
            Literal notq = new Literal(new Predicate("Q", Factory.CreateQueue<Term>()),
                    true);
            Literal notr = new Literal(new Predicate("R", Factory.CreateQueue<Term>()),
                    true);

            Chain c = new Chain();

            conts = c.getContrapositives();
            Assert.AreEqual(0, conts.Size());

            c.addLiteral(p);
            conts = c.getContrapositives();
            Assert.AreEqual(0, conts.Size());

            c.addLiteral(notq);
            conts = c.getContrapositives();
            Assert.AreEqual(1, conts.Size());
            Assert.AreEqual("<~Q(),P()>", conts.Get(0).ToString());

            c.addLiteral(notr);
            conts = c.getContrapositives();
            Assert.AreEqual(2, conts.Size());
            Assert.AreEqual("<~Q(),P(),~R()>", conts.Get(0).ToString());
            Assert.AreEqual("<~R(),P(),~Q()>", conts.Get(1).ToString());
        }
    }

}
