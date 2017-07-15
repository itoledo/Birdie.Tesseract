using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tvn.cosine.ai.logic.fol;
using tvn.cosine.ai.logic.fol.domain;
using tvn.cosine.ai.logic.fol.parsing;
using tvn.cosine.ai.logic.fol.parsing.ast;

namespace tvn_cosine.ai.test.logic.fol
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
            IList<Predicate> predicates = collector.getPredicates(s);
            Assert.IsNotNull(predicates);
        }
    } 
}
