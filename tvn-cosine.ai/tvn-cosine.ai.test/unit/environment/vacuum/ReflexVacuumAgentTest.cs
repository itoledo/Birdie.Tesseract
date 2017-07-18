﻿namespace tvn_cosine.ai.test.unit.environment.vacuum
{
    public class ReflexVacuumAgentTest
    {
        private ReflexVacuumAgent agent;

        private StringBuilder envChanges;

        @Before
        public void setUp()
        {
            agent = new ReflexVacuumAgent();
            envChanges = new StringBuilder();
        }

        @Test
        public void testCleanClean()
        {
            VacuumEnvironment tve = new VacuumEnvironment(
                    VacuumEnvironment.LocationState.Clean,
                    VacuumEnvironment.LocationState.Clean);
            tve.addAgent(agent, VacuumEnvironment.LOCATION_A);

            tve.addEnvironmentView(new VacuumEnvironmentViewActionTracker(envChanges));

            tve.step(8);

            Assert.assertEquals(
                    "Action[name==Right]Action[name==Left]Action[name==Right]Action[name==Left]Action[name==Right]Action[name==Left]Action[name==Right]Action[name==Left]",
                    envChanges.toString());
        }

        @Test
        public void testCleanDirty()
        {
            VacuumEnvironment tve = new VacuumEnvironment(
                    VacuumEnvironment.LocationState.Clean,
                    VacuumEnvironment.LocationState.Dirty);
            tve.addAgent(agent, VacuumEnvironment.LOCATION_A);

            tve.addEnvironmentView(new VacuumEnvironmentViewActionTracker(envChanges));

            tve.step(8);

            Assert.assertEquals(
                    "Action[name==Right]Action[name==Suck]Action[name==Left]Action[name==Right]Action[name==Left]Action[name==Right]Action[name==Left]Action[name==Right]",
                    envChanges.toString());
        }

        @Test
        public void testDirtyClean()
        {
            VacuumEnvironment tve = new VacuumEnvironment(
                    VacuumEnvironment.LocationState.Dirty,
                    VacuumEnvironment.LocationState.Clean);
            tve.addAgent(agent, VacuumEnvironment.LOCATION_A);

            tve.addEnvironmentView(new VacuumEnvironmentViewActionTracker(envChanges));

            tve.step(8);

            Assert.assertEquals(
                    "Action[name==Suck]Action[name==Right]Action[name==Left]Action[name==Right]Action[name==Left]Action[name==Right]Action[name==Left]Action[name==Right]",
                    envChanges.toString());
        }

        @Test
        public void testDirtyDirty()
        {
            VacuumEnvironment tve = new VacuumEnvironment(
                    VacuumEnvironment.LocationState.Dirty,
                    VacuumEnvironment.LocationState.Dirty);
            tve.addAgent(agent, VacuumEnvironment.LOCATION_A);

            tve.addEnvironmentView(new VacuumEnvironmentViewActionTracker(envChanges));

            tve.step(8);

            Assert.assertEquals(
                    "Action[name==Suck]Action[name==Right]Action[name==Suck]Action[name==Left]Action[name==Right]Action[name==Left]Action[name==Right]Action[name==Left]",
                    envChanges.toString());
        }
    }

}