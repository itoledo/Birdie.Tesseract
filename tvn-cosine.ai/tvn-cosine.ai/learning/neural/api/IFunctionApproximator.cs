using tvn.cosine.ai.util.math;

namespace tvn.cosine.ai.learning.neural.api
{
    public interface IFunctionApproximator
    {  
        /// <summary>
        /// Returns the output values for the specified input values
        /// </summary>
        /// <param name="input">the input values</param>
        /// <returns>the output values for the specified input values</returns>
        Vector ProcessInput(Vector input);
       
        /// <summary>
        /// Accept an error and change the parameters to accommodate it
        /// </summary>
        /// <param name="error">an error vector</param>
        void ProcessError(Vector error);
    }
}
