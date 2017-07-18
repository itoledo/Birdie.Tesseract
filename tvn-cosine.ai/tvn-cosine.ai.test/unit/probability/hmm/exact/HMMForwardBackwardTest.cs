namespace tvn_cosine.ai.test.unit.probability.hmm.exact
{
    public class HMMForwardBackwardTest extends CommonForwardBackwardTest
    {

    //
    private HMMForwardBackward uw = null;

    @Before
    public void setUp()
    {
        uw = new HMMForwardBackward(HMMExampleFactory.getUmbrellaWorldModel());
    }

    @Test
    public void testForwardStep_UmbrellaWorld()
    {
        super.testForwardStep_UmbrellaWorld(uw);
    }

    @Test
    public void testBackwardStep_UmbrellaWorld()
    {
        super.testBackwardStep_UmbrellaWorld(uw);
    }

    @Test
    public void testForwardBackward_UmbrellaWorld()
    {
        super.testForwardBackward_UmbrellaWorld(uw);
    }
}

}
