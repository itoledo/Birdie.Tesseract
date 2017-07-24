using tvn.cosine.ai.common.collections.api;
using tvn.cosine.ai.common.datastructures;
using tvn.cosine.ai.learning.framework;

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
        Pair<ICollection<double>, ICollection<double>> numerize(Example e);

        string denumerize(ICollection<double> outputValue);
    }
}
