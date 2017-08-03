using tvn.cosine.ai.util.math;

namespace tvn.cosine.ai.learning.neural.api
{
    public interface INeuralNetworkTrainingScheme
    {
        Vector ProcessInput(IFunctionApproximator network, Vector input); 
        void ProcessError(IFunctionApproximator network, Vector error); 
        void SetNeuralNetwork(IFunctionApproximator ffnn);
    }
}
