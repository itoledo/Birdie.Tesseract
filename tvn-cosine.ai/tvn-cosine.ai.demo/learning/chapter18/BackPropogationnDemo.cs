using tvn.cosine.exceptions;
using tvn.cosine.ai.learning.framework;
using tvn.cosine.ai.learning.neural;
using tvn.cosine.ai.learning.neural.api;
using tvn.cosine.ai.learning.neural.examples;
using tvn.cosine.ai.util;

namespace tvn_cosine.ai.demo.learning.chapter18
{
    public class BackPropogationnDemo
    {
        static void Main(params string[] args)
        {
            System.Console.WriteLine(Util.ntimes("*", 100));
            System.Console.WriteLine(
                "\n BackpropagationnDemo  - Running BackProp n hidden layers on Iris data Set with 1000 epochs of learning ");
            System.Console.WriteLine(Util.ntimes("*", 100));
            backPropogationnDemo();

            BackPropogationDemo.Main();

            System.Console.ReadLine();
        }

        internal static void backPropogationnDemo()
        {
            try
            {
                DataSet irisDataSet = DataSetFactory.getIrisDataSet();
                INumerizer numerizer = new IrisDataSetNumerizer();
                NeuralNetworkDataSet innds = new IrisNeuralNetworkDataSet();

                innds.CreateExamplesFromDataSet(irisDataSet, numerizer);

                NeuralNetworkConfig config = new NeuralNetworkConfig();
                config.SetConfig(FeedForwardDeepNeuralNetwork.NUMBER_OF_INPUTS, 4);
                config.SetConfig(FeedForwardDeepNeuralNetwork.NUMBER_OF_OUTPUTS, 3);
                config.SetConfig(FeedForwardDeepNeuralNetwork.NUMBER_OF_HIDDEN_LAYERS, 6);
                config.SetConfig(FeedForwardDeepNeuralNetwork.NUMBER_OF_HIDDEN_NEURONS_PER_LAYER, 6);
                config.SetConfig(FeedForwardDeepNeuralNetwork.LOWER_LIMIT_WEIGHTS, -2.0);
                config.SetConfig(FeedForwardDeepNeuralNetwork.UPPER_LIMIT_WEIGHTS, 2.0);

                FeedForwardDeepNeuralNetwork ffnn = new FeedForwardDeepNeuralNetwork(config, new LogSigActivationFunction());
                ffnn.SetTrainingScheme(new BackPropagationDeepLearning(0.1, 0.9));

                ffnn.TrainOn(innds, 1000);

                innds.RefreshDataset();
                int[] result = ffnn.TestOnDataSet(innds);
                System.Console.WriteLine(result[0] + " right, " + result[1] + " wrong");
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
