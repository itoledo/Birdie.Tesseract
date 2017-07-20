using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using tvn.cosine.ai.environment.vacuum;

namespace tvn_cosine.ai.test.unit.environment.vacuum
{
    [TestClass]
    public class ModelBasedReflexVacuumAgentTest
    {
        private ModelBasedReflexVacuumAgent agent;

        private StringBuilder envChanges;

        [TestInitialize]
        public void setUp()
        {
            agent = new ModelBasedReflexVacuumAgent();
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

            tve.stepUntilDone();

            Assert.AreEqual("Action[name==Right]Action[name==NoOp]",
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

            tve.stepUntilDone();

            Assert.AreEqual(
                    "Action[name==Right]Action[name==Suck]Action[name==NoOp]",
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

            tve.stepUntilDone();

            Assert.AreEqual(
                    "Action[name==Suck]Action[name==Right]Action[name==NoOp]",
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

            tve.stepUntilDone();

            Assert.AreEqual(
                    "Action[name==Suck]Action[name==Right]Action[name==Suck]Action[name==NoOp]",
                    envChanges.ToString());
        }

        [TestMethod]
        public void testAgentActionNumber1()
        {
            VacuumEnvironment tve = new VacuumEnvironment(
                    VacuumEnvironment.LocationState.Dirty,
                    VacuumEnvironment.LocationState.Dirty);
            tve.addAgent(agent, VacuumEnvironment.LOCATION_A);

            Assert.AreEqual(VacuumEnvironment.LOCATION_A,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(1, tve.getAgents().Size());
            tve.step(); // cleans location A
            Assert.AreEqual(VacuumEnvironment.LOCATION_A,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_A));
            tve.step(); // moves to lcation B
            Assert.AreEqual(VacuumEnvironment.LOCATION_B,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(VacuumEnvironment.LocationState.Dirty,
                    tve.getLocationState(VacuumEnvironment.LOCATION_B));
            tve.step(); // cleans location B
            Assert.AreEqual(VacuumEnvironment.LOCATION_B,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_B));
            tve.step(); // NOOP
            Assert.AreEqual(VacuumEnvironment.LOCATION_B,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(19, tve.getPerformanceMeasure(agent), 0.001);
        }

        [TestMethod]
        public void testAgentActionNumber2()
        {
            VacuumEnvironment tve = new VacuumEnvironment(
                    VacuumEnvironment.LocationState.Dirty,
                    VacuumEnvironment.LocationState.Dirty);
            tve.addAgent(agent, VacuumEnvironment.LOCATION_B);

            Assert.AreEqual(VacuumEnvironment.LOCATION_B,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(1, tve.getAgents().Size());
            tve.step(); // cleans location B
            Assert.AreEqual(VacuumEnvironment.LOCATION_B,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_B));
            tve.step(); // moves to lcation A
            Assert.AreEqual(VacuumEnvironment.LOCATION_A,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(VacuumEnvironment.LocationState.Dirty,
                    tve.getLocationState(VacuumEnvironment.LOCATION_A));
            tve.step(); // cleans location A
            Assert.AreEqual(VacuumEnvironment.LOCATION_A,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_A));
            tve.step(); // NOOP
            Assert.AreEqual(VacuumEnvironment.LOCATION_A,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_A));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_B));
            Assert.AreEqual(19, tve.getPerformanceMeasure(agent), 0.001);
        }

        [TestMethod]
        public void testAgentActionNumber3()
        {
            VacuumEnvironment tve = new VacuumEnvironment(
                    VacuumEnvironment.LocationState.Clean,
                    VacuumEnvironment.LocationState.Clean);
            tve.addAgent(agent, VacuumEnvironment.LOCATION_A);

            Assert.AreEqual(VacuumEnvironment.LOCATION_A,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(1, tve.getAgents().Size());
            tve.step(); // moves to location B
            Assert.AreEqual(VacuumEnvironment.LOCATION_B,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_B));
            tve.step(); // NOOP
            Assert.AreEqual(VacuumEnvironment.LOCATION_B,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_A));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_B));
            Assert.AreEqual(-1, tve.getPerformanceMeasure(agent), 0.001);
        }

        [TestMethod]
        public void testAgentActionNumber4()
        {
            VacuumEnvironment tve = new VacuumEnvironment(
                    VacuumEnvironment.LocationState.Clean,
                    VacuumEnvironment.LocationState.Clean);
            tve.addAgent(agent, VacuumEnvironment.LOCATION_B);

            Assert.AreEqual(VacuumEnvironment.LOCATION_B,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(1, tve.getAgents().Size());
            tve.step(); // moves to location A
            Assert.AreEqual(VacuumEnvironment.LOCATION_A,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_A));
            tve.step(); // NOOP
            Assert.AreEqual(VacuumEnvironment.LOCATION_A,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_A));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_B));
            Assert.AreEqual(-1, tve.getPerformanceMeasure(agent), 0.001);
        }

        [TestMethod]
        public void testAgentActionNumber5()
        {
            VacuumEnvironment tve = new VacuumEnvironment(
                    VacuumEnvironment.LocationState.Clean,
                    VacuumEnvironment.LocationState.Dirty);
            tve.addAgent(agent, VacuumEnvironment.LOCATION_A);

            Assert.AreEqual(VacuumEnvironment.LOCATION_A,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(1, tve.getAgents().Size());
            tve.step(); // moves to B
            Assert.AreEqual(VacuumEnvironment.LOCATION_B,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(VacuumEnvironment.LocationState.Dirty,
                    tve.getLocationState(VacuumEnvironment.LOCATION_B));
            tve.step(); // cleans location B
            Assert.AreEqual(VacuumEnvironment.LOCATION_B,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_B));
            tve.step(); // NOOP
            Assert.AreEqual(VacuumEnvironment.LOCATION_B,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_A));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_B));
            Assert.AreEqual(9, tve.getPerformanceMeasure(agent), 0.001);
        }

        [TestMethod]
        public void testAgentActionNumber6()
        {
            VacuumEnvironment tve = new VacuumEnvironment(
                    VacuumEnvironment.LocationState.Clean,
                    VacuumEnvironment.LocationState.Dirty);
            tve.addAgent(agent, VacuumEnvironment.LOCATION_B);

            Assert.AreEqual(VacuumEnvironment.LOCATION_B,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(1, tve.getAgents().Size());
            tve.step(); // cleans B
            Assert.AreEqual(VacuumEnvironment.LOCATION_B,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_B));
            tve.step(); // moves to A
            Assert.AreEqual(VacuumEnvironment.LOCATION_A,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_A));
            tve.step(); // NOOP
            Assert.AreEqual(VacuumEnvironment.LOCATION_A,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_A));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_B));
            Assert.AreEqual(9, tve.getPerformanceMeasure(agent), 0.001);
        }

        [TestMethod]
        public void testAgentActionNumber7()
        {
            VacuumEnvironment tve = new VacuumEnvironment(
                    VacuumEnvironment.LocationState.Dirty,
                    VacuumEnvironment.LocationState.Clean);
            tve.addAgent(agent, VacuumEnvironment.LOCATION_A);

            Assert.AreEqual(VacuumEnvironment.LOCATION_A,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(1, tve.getAgents().Size());
            tve.step(); // cleans A
            Assert.AreEqual(VacuumEnvironment.LOCATION_A,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_A));
            tve.step(); // moves to B
            Assert.AreEqual(VacuumEnvironment.LOCATION_B,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_B));
            tve.step(); // NOOP
            Assert.AreEqual(VacuumEnvironment.LOCATION_B,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_A));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_B));
            Assert.AreEqual(9, tve.getPerformanceMeasure(agent), 0.001);
        }

        [TestMethod]
        public void testAgentActionNumber8()
        {
            VacuumEnvironment tve = new VacuumEnvironment(
                    VacuumEnvironment.LocationState.Dirty,
                    VacuumEnvironment.LocationState.Clean);
            tve.addAgent(agent, VacuumEnvironment.LOCATION_B);

            Assert.AreEqual(VacuumEnvironment.LOCATION_B,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(1, tve.getAgents().Size());
            tve.step(); // moves to A
            Assert.AreEqual(VacuumEnvironment.LOCATION_A,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(VacuumEnvironment.LocationState.Dirty,
                    tve.getLocationState(VacuumEnvironment.LOCATION_A));
            tve.step(); // cleans A
            Assert.AreEqual(VacuumEnvironment.LOCATION_A,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_A));
            tve.step(); // NOOP
            Assert.AreEqual(VacuumEnvironment.LOCATION_A,
                    tve.getAgentLocation(agent));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_A));
            Assert.AreEqual(VacuumEnvironment.LocationState.Clean,
                    tve.getLocationState(VacuumEnvironment.LOCATION_B));
            Assert.AreEqual(9, tve.getPerformanceMeasure(agent), 0.001);
        }
    }
}
