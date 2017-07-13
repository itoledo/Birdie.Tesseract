using System.Collections.Generic;
using tvn.cosine.ai.learning.framework;
using tvn.cosine.ai.util.datastructure;

namespace tvn.cosine.ai.learning.neural
{
    /**
     * A Numerizer understands how to convert an example from a particular data set
     * into a <code>Pair</code> of lists of doubles. The first represents the input
     * to the neural network, and the second represents the desired output. See
     * <code>IrisDataSetNumerizer</code> for a concrete example
     * 
     * @author Ravi Mohan
     * 
     */
    public interface Numerizer
    {
        Pair<IList<double>, IList<double>> numerize(Example e);

        string denumerize(IList<double> outputValue);
    }
}
