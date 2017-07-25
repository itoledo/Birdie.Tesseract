using tvn.cosine.ai.learning.framework;
using tvn.cosine.ai.learning.neural.api;
using tvn.cosine.ai.util.math;

namespace tvn.cosine.ai.learning.neural
{
    public class FeedForwardNeuralNetwork : IFunctionApproximator
    {
        public const string UPPER_LIMIT_WEIGHTS = "upper_limit_weights";
        public const string LOWER_LIMIT_WEIGHTS = "lower_limit_weights";
        public const string NUMBER_OF_OUTPUTS = "number_of_outputs";
        public const string NUMBER_OF_HIDDEN_NEURONS = "number_of_hidden_neurons";
        public const string NUMBER_OF_INPUTS = "number_of_inputs";
        //
        private readonly Layer hiddenLayer;
        private readonly Layer outputLayer;

        private INeuralNetworkTrainingScheme trainingScheme;

        /// <summary>
        /// Constructor to be used for non testing code.
        /// </summary>
        /// <param name="config"></param>
        public FeedForwardNeuralNetwork(NeuralNetworkConfig config)
        {

            int numberOfInputNeurons = config
                    .getParameterAsInteger(NUMBER_OF_INPUTS);
            int numberOfHiddenNeurons = config
                    .getParameterAsInteger(NUMBER_OF_HIDDEN_NEURONS);
            int numberOfOutputNeurons = config
                    .getParameterAsInteger(NUMBER_OF_OUTPUTS);

            double lowerLimitForWeights = config
                    .getParameterAsDouble(LOWER_LIMIT_WEIGHTS);
            double upperLimitForWeights = config
                    .getParameterAsDouble(UPPER_LIMIT_WEIGHTS);

            hiddenLayer = new Layer(numberOfHiddenNeurons, numberOfInputNeurons,
                    lowerLimitForWeights, upperLimitForWeights,
                    new LogSigActivationFunction());

            outputLayer = new Layer(numberOfOutputNeurons, numberOfHiddenNeurons,
                    lowerLimitForWeights, upperLimitForWeights,
                    new PureLinearActivationFunction());

        }

        /// <summary>
        /// ONLY for testing to set up a network with known weights in future use to
        /// deserialize networks after adding variables for pen weightupdate,
        /// lastnput etc
        /// </summary>
        /// <param name="hiddenLayerWeights"></param>
        /// <param name="hiddenLayerBias"></param>
        /// <param name="outputLayerWeights"></param>
        /// <param name="outputLayerBias"></param>
        public FeedForwardNeuralNetwork(Matrix hiddenLayerWeights,
                Vector hiddenLayerBias, Matrix outputLayerWeights,
                Vector outputLayerBias)
        {

            hiddenLayer = new Layer(hiddenLayerWeights, hiddenLayerBias, new LogSigActivationFunction());
            outputLayer = new Layer(outputLayerWeights, outputLayerBias, new PureLinearActivationFunction());

        }

        public void ProcessError(Vector error)
        {
            trainingScheme.processError(this, error);
        }

        public Vector ProcessInput(Vector input)
        {
            return trainingScheme.processInput(this, input);
        }

        public void trainOn(NeuralNetworkDataSet innds, int numberofEpochs)
        {
            for (int i = 0; i < numberofEpochs; ++i)
            {
                innds.refreshDataset();
                while (innds.hasMoreExamples())
                {
                    NeuralNetworkExample nne = innds.getExampleAtRandom();
                    ProcessInput(nne.getInput());
                    Vector error = getOutputLayer()
                            .errorVectorFrom(nne.getTarget());
                    ProcessError(error);
                }
            }
        }

        public Vector predict(NeuralNetworkExample nne)
        {
            return ProcessInput(nne.getInput());
        }

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

        public virtual void testOn(DataSet ds)
        {

        }

        public Matrix getHiddenLayerWeights()
        {

            return hiddenLayer.getWeightMatrix();
        }

        public Vector getHiddenLayerBias()
        {

            return hiddenLayer.getBiasVector();
        }

        public Matrix getOutputLayerWeights()
        {

            return outputLayer.getWeightMatrix();
        }

        public Vector getOutputLayerBias()
        {

            return outputLayer.getBiasVector();
        }

        public Layer getHiddenLayer()
        {
            return hiddenLayer;
        }

        public Layer getOutputLayer()
        {
            return outputLayer;
        }

        public void setTrainingScheme(INeuralNetworkTrainingScheme trainingScheme)
        {
            this.trainingScheme = trainingScheme;
            trainingScheme.setNeuralNetwork(this);
        }
    }
}
