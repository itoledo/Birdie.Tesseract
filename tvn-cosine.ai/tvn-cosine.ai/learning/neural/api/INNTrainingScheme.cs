using tvn.cosine.ai.learning.neural.api;
using tvn.cosine.ai.util.math;

namespace tvn.cosine.ai.learning.neural.api
{
    public interface INNTrainingScheme
    {
        Vector processInput(FeedForwardNeuralNetwork network, Vector input);

        void processError(FeedForwardNeuralNetwork network, Vector error);

        void setNeuralNetwork(IFunctionApproximator ffnn);
    }
}
