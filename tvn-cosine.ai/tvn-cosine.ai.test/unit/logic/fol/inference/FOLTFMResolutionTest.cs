using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.ai.logic.fol.inference;

namespace tvn_cosine.ai.test.unit.logic.fol.inference
{
    [TestClass]
    public class FOLTFMResolutionTest : CommonFOLInferenceProcedureTests
    { 
        [TestMethod]
        [Ignore]
        public void testDefiniteClauseKBKingsQueryCriminalXFalse()
        {
            testDefiniteClauseKBKingsQueryCriminalXFalse(new FOLTFMResolution());
        }

        [TestMethod]
        [Ignore]
        public void testDefiniteClauseKBKingsQueryRichardEvilFalse()
        {
            testDefiniteClauseKBKingsQueryRichardEvilFalse(new FOLTFMResolution());
        }

        [TestMethod]
        [Ignore]
        public void testDefiniteClauseKBKingsQueryJohnEvilSucceeds()
        {
            testDefiniteClauseKBKingsQueryJohnEvilSucceeds(new FOLTFMResolution());
        }

        [TestMethod]
        [Ignore]
        public void testDefiniteClauseKBKingsQueryEvilXReturnsJohnSucceeds()
        {
            testDefiniteClauseKBKingsQueryEvilXReturnsJohnSucceeds(new FOLTFMResolution());
        }

        [TestMethod]
        [Ignore]
        public void testDefiniteClauseKBKingsQueryKingXReturnsJohnAndRichardSucceeds()
        {
            testDefiniteClauseKBKingsQueryKingXReturnsJohnAndRichardSucceeds(new FOLTFMResolution());
        }

        [TestMethod]
        [Ignore]
        public void testDefiniteClauseKBWeaponsQueryCriminalXReturnsWestSucceeds()
        {
            testDefiniteClauseKBWeaponsQueryCriminalXReturnsWestSucceeds(new FOLTFMResolution());
        }

        [TestMethod]
        [Ignore]
        public void testHornClauseKBRingOfThievesQuerySkisXReturnsNancyRedBertDrew()
        {
            // The clauses in this KB can keep creating resolvents infinitely,
            // therefore give it 40 seconds to find the 4 answers to this, should
            // be more than enough.
            testHornClauseKBRingOfThievesQuerySkisXReturnsNancyRedBertDrew(new FOLTFMResolution(
                    40 * 1000));
        }

        [TestMethod]
        [Ignore]
        public void testFullFOLKBLovesAnimalQueryKillsCuriosityTunaSucceeds()
        {
            // 10 seconds should be more than plenty for this query to finish.
            testFullFOLKBLovesAnimalQueryKillsCuriosityTunaSucceeds(
                    new FOLTFMResolution(10 * 1000), false);
        }

        [TestMethod]
        [Ignore]
        public void testFullFOLKBLovesAnimalQueryNotKillsJackTunaSucceeds()
        {
            // 10 seconds should be more than plenty for this query to finish.
            testFullFOLKBLovesAnimalQueryNotKillsJackTunaSucceeds(
                    new FOLTFMResolution(10 * 1000), false);
        }

        [TestMethod]
        public void testFullFOLKBLovesAnimalQueryKillsJackTunaFalse()
        {
            // This query will not return using TFM as keep expanding
            // clauses through resolution for this KB.
            testFullFOLKBLovesAnimalQueryKillsJackTunaFalse(new FOLTFMResolution(
                    10 * 1000), true);
        }

        [TestMethod]
        [Ignore]
        public void testEqualityAxiomsKBabcAEqualsCSucceeds()
        {
            testEqualityAxiomsKBabcAEqualsCSucceeds(new FOLTFMResolution(10 * 1000));
        }

        [TestMethod]
        [Ignore]
        public void testEqualityAndSubstitutionAxiomsKBabcdFFASucceeds()
        {
            testEqualityAndSubstitutionAxiomsKBabcdFFASucceeds(new FOLTFMResolution(
                    40 * 1000));
        }

    ////    // Note: Requires VM arguments to be:
    ////    // -Xms256m -Xmx1024m
    ////    // due to the amount of memory it uses.
    ////    // Therefore, ignore by default as people don't normally set this.
    ////    [Ignore]
    ////    [TestMethod] 
    ////public void testEqualityAndSubstitutionAxiomsKBabcdPDSucceeds()
    ////    {
    ////        testEqualityAndSubstitutionAxiomsKBabcdPDSucceeds(new FOLTFMResolution(
    ////                10 * 1000));
    ////    }

        [TestMethod]
        [Ignore]
        public void testEqualityAndSubstitutionAxiomsKBabcdPFFASucceeds()
        {
            // TFM is unable to find the correct answer to this in a reasonable
            // amount of time for a JUnit test.
            testEqualityAndSubstitutionAxiomsKBabcdPFFASucceeds(
                    new FOLTFMResolution(10 * 1000), true);
        }

        [TestMethod]
        [Ignore]
        public void testEqualityNoAxiomsKBabcAEqualsCSucceeds()
        {
            testEqualityNoAxiomsKBabcAEqualsCSucceeds(new FOLTFMResolution(
                    10 * 1000), true);
        }

        [TestMethod]
        [Ignore]
        public void testEqualityAndSubstitutionNoAxiomsKBabcdFFASucceeds()
        {
            testEqualityAndSubstitutionNoAxiomsKBabcdFFASucceeds(
                    new FOLTFMResolution(10 * 1000), true);
        }

        [TestMethod]
        [Ignore]
        public void testEqualityAndSubstitutionNoAxiomsKBabcdPDSucceeds()
        {
            testEqualityAndSubstitutionNoAxiomsKBabcdPDSucceeds(
                    new FOLTFMResolution(10 * 1000), true);
        }

        [TestMethod]
        [Ignore]
        public void testEqualityAndSubstitutionNoAxiomsKBabcdPFFASucceeds()
        {
            testEqualityAndSubstitutionNoAxiomsKBabcdPFFASucceeds(
                    new FOLTFMResolution(10 * 1000), true);
        }
    }
}
