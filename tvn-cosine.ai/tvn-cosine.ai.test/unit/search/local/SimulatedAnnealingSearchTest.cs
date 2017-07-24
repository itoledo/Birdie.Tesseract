using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.ai.agent.api;
using tvn.cosine.ai.search.local;

namespace tvn_cosine.ai.test.unit.search.local
{
    [TestClass] public class SimulatedAnnealingSearchTest
    {

        [TestMethod]
        public void testForGivenNegativeDeltaEProbabilityOfAcceptanceDecreasesWithDecreasingTemperature()
        {
            // this isn't very nice. the object's state is uninitialized but is ok
            // for this test.
            SimulatedAnnealingSearch<string, IAction> search = new SimulatedAnnealingSearch<string, IAction>(null);
            int deltaE = -1;
            double higherTemperature = 30.0;
            double lowerTemperature = 29.5;

            Assert.IsTrue(search.probabilityOfAcceptance(lowerTemperature,
                    deltaE) < search.probabilityOfAcceptance(higherTemperature,
                    deltaE));
        }

    }

}
