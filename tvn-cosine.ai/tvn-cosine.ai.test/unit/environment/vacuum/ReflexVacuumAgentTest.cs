using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using tvn.cosine.ai.environment.vacuum;

namespace tvn_cosine.ai.test.unit.environment.vacuum
{
    [TestClass]
    public class ReflexVacuumAgentTest
    {
        private ReflexVacuumAgent agent;

        private StringBuilder envChanges;

        [TestInitialize]
        public void setUp()
        {
            agent = new ReflexVacuumAgent();
            envChanges = new StringBuilder();
        }

        [TestMethod]
        public void testCleanClean()
        {
            VacuumEnvironment tve = new VacuumEnvironment(
                    VacuumEnvironment.LocationState.Clean,
                    VacuumEnvironment.LocationState.Clean);
            tve.addAgent(agent, VacuumEnvironment.LOCATION_A);

            tve.addEnvironmentView(new VacuumEnvironmentViewActionTracker(envChanges));

            tve.step(8);

            Assert.AreEqual(
                    "Action[name==Right]Action[name==Left]Action[name==Right]Action[name==Left]Action[name==Right]Action[name==Left]Action[name==Right]Action[name==Left]",
                    envChanges.ToString());
        }

        [TestMethod]
        public void testCleanDirty()
        {
            VacuumEnvironment tve = new VacuumEnvironment(
                    VacuumEnvironment.LocationState.Clean,
                    VacuumEnvironment.LocationState.Dirty);
            tve.addAgent(agent, VacuumEnvironment.LOCATION_A);

            tve.addEnvironmentView(new VacuumEnvironmentViewActionTracker(envChanges));

            tve.step(8);

            Assert.AreEqual(
                    "Action[name==Right]Action[name==Suck]Action[name==Left]Action[name==Right]Action[name==Left]Action[name==Right]Action[name==Left]Action[name==Right]",
                    envChanges.ToString());
        }

        [TestMethod]
        public void testDirtyClean()
        {
            VacuumEnvironment tve = new VacuumEnvironment(
                    VacuumEnvironment.LocationState.Dirty,
                    VacuumEnvironment.LocationState.Clean);
            tve.addAgent(agent, VacuumEnvironment.LOCATION_A);

            tve.addEnvironmentView(new VacuumEnvironmentViewActionTracker(envChanges));

            tve.step(8);

            Assert.AreEqual(
                    "Action[name==Suck]Action[name==Right]Action[name==Left]Action[name==Right]Action[name==Left]Action[name==Right]Action[name==Left]Action[name==Right]",
                    envChanges.ToString());
        }

        [TestMethod]
        public void testDirtyDirty()
        {
            VacuumEnvironment tve = new VacuumEnvironment(
                    VacuumEnvironment.LocationState.Dirty,
                    VacuumEnvironment.LocationState.Dirty);
            tve.addAgent(agent, VacuumEnvironment.LOCATION_A);

            tve.addEnvironmentView(new VacuumEnvironmentViewActionTracker(envChanges));

            tve.step(8);

            Assert.AreEqual(
                    "Action[name==Suck]Action[name==Right]Action[name==Suck]Action[name==Left]Action[name==Right]Action[name==Left]Action[name==Right]Action[name==Left]",
                    envChanges.ToString());
        }
    }

}
