using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.ai.logic.fol.inference;

namespace tvn_cosine.ai.test.unit.logic.fol.inference
{
    [TestClass]
    public class FOLBCAskTest : CommonFOLInferenceProcedureTests
    {

        [TestMethod]
        [Ignore]
        public void testDefiniteClauseKBKingsQueryCriminalXFalse()
        {
            testDefiniteClauseKBKingsQueryCriminalXFalse(new FOLBCAsk());
        }

        [TestMethod]
        [Ignore]
        public void testDefiniteClauseKBKingsQueryRichardEvilFalse()
        {
            testDefiniteClauseKBKingsQueryRichardEvilFalse(new FOLBCAsk());
        }

        [TestMethod]
        [Ignore]
        public void testDefiniteClauseKBKingsQueryJohnEvilSucceeds()
        {
            testDefiniteClauseKBKingsQueryJohnEvilSucceeds(new FOLBCAsk());
        }

        [TestMethod]
        [Ignore]
        public void testDefiniteClauseKBKingsQueryEvilXReturnsJohnSucceeds()
        {
            testDefiniteClauseKBKingsQueryEvilXReturnsJohnSucceeds(new FOLBCAsk());
        }

        [TestMethod]
        [Ignore]
        public void testDefiniteClauseKBKingsQueryKingXReturnsJohnAndRichardSucceeds()
        {
            testDefiniteClauseKBKingsQueryKingXReturnsJohnAndRichardSucceeds(new FOLBCAsk());
        }

        [TestMethod]
        [Ignore]
        public void testDefiniteClauseKBWeaponsQueryCriminalXReturnsWestSucceeds()
        {
            testDefiniteClauseKBWeaponsQueryCriminalXReturnsWestSucceeds(new FOLBCAsk());
        }
    }
}
