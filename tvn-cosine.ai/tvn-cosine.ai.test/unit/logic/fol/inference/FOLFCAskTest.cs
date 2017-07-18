namespace tvn_cosine.ai.test.unit.logic.fol.inference
{
    public class FOLFCAskTest extends CommonFOLInferenceProcedureTests
    {

        @Test

    public void testDefiniteClauseKBKingsQueryCriminalXFalse()
    {
        testDefiniteClauseKBKingsQueryCriminalXFalse(new FOLFCAsk());
    }

    @Test
    public void testDefiniteClauseKBKingsQueryRichardEvilFalse()
    {
        testDefiniteClauseKBKingsQueryRichardEvilFalse(new FOLFCAsk());
    }

    @Test
    public void testDefiniteClauseKBKingsQueryJohnEvilSucceeds()
    {
        testDefiniteClauseKBKingsQueryJohnEvilSucceeds(new FOLFCAsk());
    }

    @Test
    public void testDefiniteClauseKBKingsQueryEvilXReturnsJohnSucceeds()
    {
        testDefiniteClauseKBKingsQueryEvilXReturnsJohnSucceeds(new FOLFCAsk());
    }

    @Test
    public void testDefiniteClauseKBKingsQueryKingXReturnsJohnAndRichardSucceeds()
    {
        testDefiniteClauseKBKingsQueryKingXReturnsJohnAndRichardSucceeds(new FOLFCAsk());
    }

    @Test
    public void testDefiniteClauseKBWeaponsQueryCriminalXReturnsWestSucceeds()
    {
        testDefiniteClauseKBWeaponsQueryCriminalXReturnsWestSucceeds(new FOLFCAsk());
    }
}
}
