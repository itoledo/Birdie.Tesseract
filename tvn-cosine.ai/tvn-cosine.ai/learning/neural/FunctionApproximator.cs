using tvn.cosine.ai.util.math;

namespace tvn.cosine.ai.learning.neural
{
    public interface FunctionApproximator
    { 
        /**
         * Returns the output values for the specified input values
         * 
         * @param input
         *            the input values
         * 
         * @return the output values for the specified input values
         */
        Vector processInput(Vector input);

        /**
         * Accept an error and change the parameters to accommodate it
         * 
         * @param error
         *            an error vector
         */
        void processError(Vector error);
    }
}
