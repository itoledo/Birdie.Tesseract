namespace tvn_cosine.ai.test.unit.probability.hmm.exact
{
    public class HMMForwardBackwardConstantSpaceTest extends
            CommonForwardBackwardTest
    {

    //
    private HMMForwardBackwardConstantSpace uw = null;

    @Before
    public void setUp()
    {
        uw = new HMMForwardBackwardConstantSpace(
                HMMExampleFactory.getUmbrellaWorldModel());
    }

    @Test
    public void testForwardBackward_UmbrellaWorld()
    {
        super.testForwardBackward_UmbrellaWorld(uw);
    }
}

}
