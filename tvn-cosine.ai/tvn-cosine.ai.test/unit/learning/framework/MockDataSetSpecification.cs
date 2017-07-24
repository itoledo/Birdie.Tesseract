using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.collections.api;
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

        public override ICollection<string> getAttributeNames()
        {
            return CollectionFactory.CreateQueue<string>();
        }
    } 
}
