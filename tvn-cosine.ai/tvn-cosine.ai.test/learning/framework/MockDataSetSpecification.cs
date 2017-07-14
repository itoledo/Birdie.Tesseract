using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tvn.cosine.ai.learning.framework;

namespace tvn_cosine.ai.test.learning.framework
{
    /**
     * @author Ravi Mohan
     * 
     */
    public class MockDataSetSpecification : DataSetSpecification
    { 
    public MockDataSetSpecification(string targetAttributeName)
    {
        setTarget(targetAttributeName);
    }
         
    public override IList<string> getAttributeNames()
    {
        return new List<string>();
    }
}
}
