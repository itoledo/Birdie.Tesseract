using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.ai.logic.fol.inference;

namespace tvn_cosine.ai.test.unit.logic.fol.inference
{
    [TestClass] public class FOLFCAskTest :  CommonFOLInferenceProcedureTests
    {

        [TestMethod]

    public void testDefiniteClauseKBKingsQueryCriminalXFalse()
    {
        testDefiniteClauseKBKingsQueryCriminalXFalse(new FOLFCAsk());
    }

    [TestMethod]
    public void testDefiniteClauseKBKingsQueryRichardEvilFalse()
    {
        testDefiniteClauseKBKingsQueryRichardEvilFalse(new FOLFCAsk());
    }

    [TestMethod]
    public void testDefiniteClauseKBKingsQueryJohnEvilSucceeds()
    {
        testDefiniteClauseKBKingsQueryJohnEvilSucceeds(new FOLFCAsk());
    }

    [TestMethod]
    public void testDefiniteClauseKBKingsQueryEvilXReturnsJohnSucceeds()
    {
        testDefiniteClauseKBKingsQueryEvilXReturnsJohnSucceeds(new FOLFCAsk());
    }

    [TestMethod]
    public void testDefiniteClauseKBKingsQueryKingXReturnsJohnAndRichardSucceeds()
    {
        testDefiniteClauseKBKingsQueryKingXReturnsJohnAndRichardSucceeds(new FOLFCAsk());
    }

    [TestMethod]
    public void testDefiniteClauseKBWeaponsQueryCriminalXReturnsWestSucceeds()
    {
        testDefiniteClauseKBWeaponsQueryCriminalXReturnsWestSucceeds(new FOLFCAsk());
    }
}
}
