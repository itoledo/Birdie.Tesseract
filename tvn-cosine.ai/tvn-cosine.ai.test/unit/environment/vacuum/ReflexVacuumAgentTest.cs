using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.ai.common;
using tvn.cosine.ai.common.api;
using tvn.cosine.ai.common.text;
using tvn.cosine.ai.common.text.api;
using tvn.cosine.ai.environment.vacuum;

namespace tvn_cosine.ai.test.unit.environment.vacuum
{
    [TestClass]
    public class ReflexVacuumAgentTest
    {
        private ReflexVacuumAgent agent;

        private IStringBuilder envChanges;

        [TestInitialize]
        public void setUp()
        {
            agent = new ReflexVacuumAgent();
            envChanges = TextFactory.CreateStringBuilder();
        }

        [TestMethod]
        public void testCleanClean()
        {
            VacuumEnvironment tve = new VacuumEnvironment(
                    VacuumEnvironment.LocationState.Clean,
                    VacuumEnvironment.LocationState.Clean);
            tve.addAgent(agent, VacuumEnvironment.LOCATION_A);

            tve.AddEnvironmentView(new VacuumEnvironmentViewActionTracker(envChanges));

            tve.Step(8);

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

            tve.AddEnvironmentView(new VacuumEnvironmentViewActionTracker(envChanges));

            tve.Step(8);

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

            tve.AddEnvironmentView(new VacuumEnvironmentViewActionTracker(envChanges));

            tve.Step(8);

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

            tve.AddEnvironmentView(new VacuumEnvironmentViewActionTracker(envChanges));

            tve.Step(8);

            Assert.AreEqual(
                    "Action[name==Suck]Action[name==Right]Action[name==Suck]Action[name==Left]Action[name==Right]Action[name==Left]Action[name==Right]Action[name==Left]",
                    envChanges.ToString());
        }
    }

}
