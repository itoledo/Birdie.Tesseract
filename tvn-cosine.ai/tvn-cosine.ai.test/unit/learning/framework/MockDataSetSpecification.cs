namespace tvn_cosine.ai.test.unit.learning.framework
{
    public class MockDataSetSpecification extends DataSetSpecification
    {


    public MockDataSetSpecification(String targetAttributeName)
    {
        setTarget(targetAttributeName);
    }

    @Override
    public List<String> getAttributeNames()
    {
        return new ArrayList<String>();
    }
}

}
