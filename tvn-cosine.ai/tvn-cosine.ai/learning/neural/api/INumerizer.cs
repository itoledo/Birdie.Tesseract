using tvn.cosine.ai.common.collections.api;
using tvn.cosine.ai.common.datastructures;
using tvn.cosine.ai.learning.framework;

namespace tvn.cosine.ai.learning.neural.api
{
    /// <summary>
    /// A Numerizer understands how to convert an example from a particular data set
    /// into a Pair of lists of doubles. The first represents the input
    /// to the neural network, and the second represents the desired output. See
    /// IrisDataSetNumerizer for a concrete example
    /// </summary>
    public interface INumerizer
    {
        Pair<ICollection<double>, ICollection<double>> numerize(Example e);

        string Denumerize(ICollection<double> outputValue);
    }
}
