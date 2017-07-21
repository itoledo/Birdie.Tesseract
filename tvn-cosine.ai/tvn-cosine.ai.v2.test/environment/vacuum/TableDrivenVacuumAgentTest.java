namespace aima.test.unit.environment.vacuum;

using aima.core.environment.vacuum.TableDrivenVacuumAgent;
using aima.core.environment.vacuum.VEPercept;
using aima.core.environment.vacuum.VacuumEnvironment;

using org.junit.Assert;
using org.junit.Before;
using org.junit.Test;

/**
 * @author Ciaran O'Reilly
 */
public class TableDrivenVacuumAgentTest {
    private TableDrivenVacuumAgent agent;

    @Before
    public void setUp() {
        agent = new TableDrivenVacuumAgent();
    }

    [TestMethod]
    public void testACleanAClean() {
        Assert.assertEquals(
                VacuumEnvironment.ACTION_RIGHT,
                agent.perceive(new VEPercept(VacuumEnvironment.LOCATION_A, VacuumEnvironment.Status.Clean))
        );
        Assert.assertEquals(
                VacuumEnvironment.ACTION_RIGHT,
                agent.perceive(new VEPercept(VacuumEnvironment.LOCATION_A, VacuumEnvironment.Status.Clean))
        );
    }


    [TestMethod]
    public void testACleanBClean() {
        Assert.assertEquals(
                VacuumEnvironment.ACTION_RIGHT,
                agent.perceive(new VEPercept(VacuumEnvironment.LOCATION_A, VacuumEnvironment.Status.Clean))
        );
        Assert.assertEquals(
                VacuumEnvironment.ACTION_LEFT,
                agent.perceive(new VEPercept(VacuumEnvironment.LOCATION_B, VacuumEnvironment.Status.Clean))
        );
        Assert.assertEquals(
                VacuumEnvironment.ACTION_RIGHT,
                agent.perceive(new VEPercept(VacuumEnvironment.LOCATION_A, VacuumEnvironment.Status.Clean))
        );
    }

    [TestMethod]
    public void testACleanBDirty() {
        Assert.assertEquals(
                VacuumEnvironment.ACTION_RIGHT,
                agent.perceive(new VEPercept(VacuumEnvironment.LOCATION_A, VacuumEnvironment.Status.Clean))
        );
        Assert.assertEquals(
                VacuumEnvironment.ACTION_SUCK,
                agent.perceive(new VEPercept(VacuumEnvironment.LOCATION_B, VacuumEnvironment.Status.Dirty))
        );
        Assert.assertEquals(
                VacuumEnvironment.ACTION_LEFT,
                agent.perceive(new VEPercept(VacuumEnvironment.LOCATION_B, VacuumEnvironment.Status.Clean))
        );
        // Table is only defined for max 3 percepts in a sequence, so will generate a null.
        Assert.assertNull(
                agent.perceive(new VEPercept(VacuumEnvironment.LOCATION_A, VacuumEnvironment.Status.Clean))
        );
    }

    [TestMethod]
    public void testADirtyBClean() {
        Assert.assertEquals(
                VacuumEnvironment.ACTION_SUCK,
                agent.perceive(new VEPercept(VacuumEnvironment.LOCATION_A, VacuumEnvironment.Status.Dirty))
        );
        Assert.assertEquals(
                VacuumEnvironment.ACTION_RIGHT,
                agent.perceive(new VEPercept(VacuumEnvironment.LOCATION_A, VacuumEnvironment.Status.Clean))
        );
        Assert.assertEquals(
                VacuumEnvironment.ACTION_LEFT,
                agent.perceive(new VEPercept(VacuumEnvironment.LOCATION_B, VacuumEnvironment.Status.Clean))
        );
        // Table is only defined for max 3 percepts in a sequence, so will generate a null.
        Assert.assertNull(
                agent.perceive(new VEPercept(VacuumEnvironment.LOCATION_A, VacuumEnvironment.Status.Clean))
        );
    }

    [TestMethod]
    public void testADirtyBDirty() {
        Assert.assertEquals(
                VacuumEnvironment.ACTION_SUCK,
                agent.perceive(new VEPercept(VacuumEnvironment.LOCATION_A, VacuumEnvironment.Status.Dirty))
        );
        Assert.assertEquals(
                VacuumEnvironment.ACTION_RIGHT,
                agent.perceive(new VEPercept(VacuumEnvironment.LOCATION_A, VacuumEnvironment.Status.Clean))
        );
        Assert.assertEquals(
                VacuumEnvironment.ACTION_SUCK,
                agent.perceive(new VEPercept(VacuumEnvironment.LOCATION_B, VacuumEnvironment.Status.Dirty))
        );
        // Table is only defined for max 3 percepts in a sequence, so will generate a null.
        Assert.assertNull(
                agent.perceive(new VEPercept(VacuumEnvironment.LOCATION_A, VacuumEnvironment.Status.Clean))
        );
    }
}
