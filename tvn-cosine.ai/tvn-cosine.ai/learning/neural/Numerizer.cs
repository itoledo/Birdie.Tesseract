 namespace aima.core.learning.neural;

 

import aima.core.learning.framework.Example;
import aima.core.util.datastructure.Pair;

/**
 * A Numerizer understands how to convert an example from a particular data set
 * into a <code>Pair</code> of lists of doubles. The first represents the input
 * to the neural network, and the second represents the desired output. See
 * <code>IrisDataSetNumerizer</code> for a concrete example
 * 
 * @author Ravi Mohan
 * 
 */
public interface Numerizer {
	Pair<List<double>, List<double>> numerize(Example e);

	String denumerize(List<double> outputValue);
}
