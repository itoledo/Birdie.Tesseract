using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.logic.fol;
using tvn.cosine.ai.logic.fol.domain;
using tvn.cosine.ai.logic.fol.parsing;
using tvn.cosine.ai.logic.fol.parsing.ast;

namespace tvn_cosine.ai.test.unit.logic.fol
{
    [TestClass]
    public class PredicateCollectorTest
    {
        PredicateCollector collector;

        FOLParser parser;

        [TestInitialize]
        public void setUp()
        {
            collector = new PredicateCollector();
            parser = new FOLParser(DomainFactory.weaponsDomain());
        }

        [TestMethod]
        public void testSimpleSentence()
        {
            Sentence s = parser.parse("(Missile(x) => Weapon(x))");
            IQueue<Predicate> predicates = collector.getPredicates(s);
            Assert.IsNotNull(predicates);
        }
    }

}
