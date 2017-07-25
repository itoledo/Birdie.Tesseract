using tvn.cosine.ai.util.math;

namespace tvn.cosine.ai.learning.neural.api
{
    public interface INeuralNetworkTrainingScheme
    {
        Vector ProcessInput(FeedForwardNeuralNetwork network, Vector input); 
        void ProcessError(FeedForwardNeuralNetwork network, Vector error); 
        void SetNeuralNetwork(IFunctionApproximator ffnn);
    }
}
