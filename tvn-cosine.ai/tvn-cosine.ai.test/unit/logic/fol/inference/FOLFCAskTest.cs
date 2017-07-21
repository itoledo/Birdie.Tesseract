using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.ai.logic.fol.inference;

namespace tvn_cosine.ai.test.unit.logic.fol.inference
{
    [TestClass] public class FOLFCAskTest :  CommonFOLInferenceProcedureTests
    {

        [TestMethod]
        [Ignore]
        public void testDefiniteClauseKBKingsQueryCriminalXFalse()
    {
        testDefiniteClauseKBKingsQueryCriminalXFalse(new FOLFCAsk());
    }

    [TestMethod]
        [Ignore]
        public void testDefiniteClauseKBKingsQueryRichardEvilFalse()
    {
        testDefiniteClauseKBKingsQueryRichardEvilFalse(new FOLFCAsk());
    }

    [TestMethod]
        [Ignore]
        public void testDefiniteClauseKBKingsQueryJohnEvilSucceeds()
    {
        testDefiniteClauseKBKingsQueryJohnEvilSucceeds(new FOLFCAsk());
    }

    [TestMethod]
        [Ignore]
        public void testDefiniteClauseKBKingsQueryEvilXReturnsJohnSucceeds()
    {
        testDefiniteClauseKBKingsQueryEvilXReturnsJohnSucceeds(new FOLFCAsk());
    }

    [TestMethod]
        [Ignore]
        public void testDefiniteClauseKBKingsQueryKingXReturnsJohnAndRichardSucceeds()
    {
        testDefiniteClauseKBKingsQueryKingXReturnsJohnAndRichardSucceeds(new FOLFCAsk());
    }

    [TestMethod]
        [Ignore]
        public void testDefiniteClauseKBWeaponsQueryCriminalXReturnsWestSucceeds()
    {
        testDefiniteClauseKBWeaponsQueryCriminalXReturnsWestSucceeds(new FOLFCAsk());
    }
}
}
