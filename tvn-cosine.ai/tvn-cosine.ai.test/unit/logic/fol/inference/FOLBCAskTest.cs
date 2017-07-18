namespace tvn_cosine.ai.test.unit.logic.fol.inference
{
    public class FOLBCAskTest extends CommonFOLInferenceProcedureTests
    {

        @Test

    public void testDefiniteClauseKBKingsQueryCriminalXFalse()
    {
        testDefiniteClauseKBKingsQueryCriminalXFalse(new FOLBCAsk());
    }

    @Test
    public void testDefiniteClauseKBKingsQueryRichardEvilFalse()
    {
        testDefiniteClauseKBKingsQueryRichardEvilFalse(new FOLBCAsk());
    }

    @Test
    public void testDefiniteClauseKBKingsQueryJohnEvilSucceeds()
    {
        testDefiniteClauseKBKingsQueryJohnEvilSucceeds(new FOLBCAsk());
    }

    @Test
    public void testDefiniteClauseKBKingsQueryEvilXReturnsJohnSucceeds()
    {
        testDefiniteClauseKBKingsQueryEvilXReturnsJohnSucceeds(new FOLBCAsk());
    }

    @Test
    public void testDefiniteClauseKBKingsQueryKingXReturnsJohnAndRichardSucceeds()
    {
        testDefiniteClauseKBKingsQueryKingXReturnsJohnAndRichardSucceeds(new FOLBCAsk());
    }

    @Test
    public void testDefiniteClauseKBWeaponsQueryCriminalXReturnsWestSucceeds()
    {
        testDefiniteClauseKBWeaponsQueryCriminalXReturnsWestSucceeds(new FOLBCAsk());
    }
}
}
