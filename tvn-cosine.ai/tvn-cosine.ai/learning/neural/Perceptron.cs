using tvn.cosine.ai.learning.neural.api;
using tvn.cosine.ai.util.math;

namespace tvn.cosine.ai.learning.neural
{
    public class Perceptron : IFunctionApproximator
    {
        private readonly Layer layer;
        private Vector lastInput;

        public Perceptron(int numberOfNeurons, int numberOfInputs)
        {
            this.layer = new Layer(numberOfNeurons, numberOfInputs, 2.0, -2.0, new HardLimitActivationFunction()); 
        }

        public Vector ProcessInput(Vector input)
        {
            lastInput = input;
            return layer.feedForward(input);
        }

        public void ProcessError(Vector error)
        {
            Matrix weightUpdate = error.times(lastInput.transpose());
            layer.acceptNewWeightUpdate(weightUpdate);

            Vector biasUpdate = layer.getBiasVector().plus(error);
            layer.acceptNewBiasUpdate(biasUpdate);

        }
         
        /// <summary>
        /// Induces the layer of this perceptron from the specified set of examples
        /// </summary>
        /// <param name="innds">a set of training examples for constructing the layer of this perceptron.</param>
        /// <param name="numberofEpochs">the number of training epochs to be used.</param>
        public void trainOn(NeuralNetworkDataSet innds, int numberofEpochs)
        {
            for (int i = 0; i < numberofEpochs; ++i)
            {
                innds.refreshDataset();
                while (innds.hasMoreExamples())
                {
                    NeuralNetworkExample nne = innds.getExampleAtRandom();
                    ProcessInput(nne.getInput());
                    Vector error = layer.errorVectorFrom(nne.getTarget());
                    ProcessError(error);
                }
            }
        }
         
        /// <summary>
        /// Returns the outcome predicted for the specified example
        /// </summary>
        /// <param name="nne">an example</param>
        /// <returns>the outcome predicted for the specified example</returns>
        public Vector predict(NeuralNetworkExample nne)
        {
            return ProcessInput(nne.getInput());
        }
       
        /// <summary>
        /// Returns the accuracy of the hypothesis on the specified set of examples
        /// </summary>
        /// <param name="nnds">the neural network data set to be tested on.</param>
        /// <returns>the accuracy of the hypothesis on the specified set of examples</returns>
        public int[] testOnDataSet(NeuralNetworkDataSet nnds)
        {
            int[] result = new int[] { 0, 0 };
            nnds.refreshDataset();
            while (nnds.hasMoreExamples())
            {
                NeuralNetworkExample nne = nnds.getExampleAtRandom();
                Vector prediction = predict(nne);
                if (nne.isCorrect(prediction))
                {
                    result[0] = result[0] + 1;
                }
                else
                {
                    result[1] = result[1] + 1;
                }
            }
            return result;
        }
    }
}
