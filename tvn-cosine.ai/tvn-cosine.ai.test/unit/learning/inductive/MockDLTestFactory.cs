namespace tvn_cosine.ai.test.unit.learning.inductive
{
    public class MockDLTestFactory extends DLTestFactory
    {


    private List<DLTest> tests;

    public MockDLTestFactory(List<DLTest> tests)
    {
        this.tests = tests;
    }

    @Override
    public List<DLTest> createDLTestsWithAttributeCount(DataSet ds, int i)
    {
        return tests;
    }
}

}
