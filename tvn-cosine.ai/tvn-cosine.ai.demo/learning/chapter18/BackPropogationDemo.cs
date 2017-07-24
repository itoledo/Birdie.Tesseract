using tvn.cosine.ai.common.exceptions;
using tvn.cosine.ai.learning.framework;
using tvn.cosine.ai.learning.neural;
using tvn.cosine.ai.util;

namespace tvn_cosine.ai.demo.learning.chapter18
{
    public class BackPropogationDemo
    {
        static void Main(params string[] args)
        {
            System.Console.WriteLine(Util.ntimes("*", 100));
            System.Console.WriteLine("\n BackpropagationDemo  - Running BackProp on Iris data Set with 10 epochs of learning ");
            System.Console.WriteLine(Util.ntimes("*", 100));
            backPropogationDemo();
        }

        static void backPropogationDemo()
        {
            try
            {
                DataSet irisDataSet = DataSetFactory.getIrisDataSet();
                Numerizer numerizer = new IrisDataSetNumerizer();
                NNDataSet innds = new IrisNNDataSet();

                innds.createExamplesFromDataSet(irisDataSet, numerizer);

                NNConfig config = new NNConfig();
                config.setConfig(FeedForwardNeuralNetwork.NUMBER_OF_INPUTS, 4);
                config.setConfig(FeedForwardNeuralNetwork.NUMBER_OF_OUTPUTS, 3);
                config.setConfig(FeedForwardNeuralNetwork.NUMBER_OF_HIDDEN_NEURONS,
                        6);
                config.setConfig(FeedForwardNeuralNetwork.LOWER_LIMIT_WEIGHTS, -2.0);
                config.setConfig(FeedForwardNeuralNetwork.UPPER_LIMIT_WEIGHTS, 2.0);

                FeedForwardNeuralNetwork ffnn = new FeedForwardNeuralNetwork(config);
                ffnn.setTrainingScheme(new BackPropLearning(0.1, 0.9));

                ffnn.trainOn(innds, 10);

                innds.refreshDataset();
                int[] result = ffnn.testOnDataSet(innds);
                System.Console.WriteLine(result[0] + " right, " + result[1] + " wrong");
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
