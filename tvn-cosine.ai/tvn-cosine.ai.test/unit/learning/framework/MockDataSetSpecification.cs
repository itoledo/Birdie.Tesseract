using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.learning.framework;

namespace tvn_cosine.ai.test.unit.learning.framework
{
    [TestClass]
    public class MockDataSetSpecification : DataSetSpecification
    { 
        public MockDataSetSpecification(string targetAttributeName)
        {
            setTarget(targetAttributeName);
        }

        public override IQueue<string> getAttributeNames()
        {
            return Factory.CreateQueue<string>();
        }
    } 
}
