using tvn.cosine.exceptions;
using tvn.cosine.ai.learning.framework;
using tvn.cosine.ai.learning.neural;
using tvn.cosine.ai.learning.neural.api;
using tvn.cosine.ai.learning.neural.examples;
using tvn.cosine.ai.util;

namespace tvn_cosine.ai.demo.learning.chapter18
{
    public class BackPropogationDeepLearningDemo
    {
        static int epochs = 1000;
        static int numHiddenLayers = 10;
        static int numNeuronsPerLayer = 20;

        static void Main(params string[] args)
        {
            backPropogationDeepLearningDemo();

            backPropogationDemo();
            System.Console.ReadLine();
        }

        internal static void backPropogationDeepLearningDemo()
        {
            try
            {
                System.Console.WriteLine(Util.ntimes("*", 100));
                System.Console.WriteLine(
                    "\n BackpropagationnDemo  - Running BackProp {1} hidden layers on Iris data Set with {0} epochs of learning ",
                    epochs, numHiddenLayers);
                System.Console.WriteLine(Util.ntimes("*", 100));

                DataSet animalDataSet = DataSetFactory.getAnimalDataSet();
                INumerizer numerizer = new AnimalDataSetNumerizer();
                NeuralNetworkDataSet innds = new IrisNeuralNetworkDataSet();

                innds.CreateExamplesFromDataSet(animalDataSet, numerizer);

                NeuralNetworkConfig config = new NeuralNetworkConfig();
                config.SetConfig(FeedForwardDeepNeuralNetwork.NUMBER_OF_INPUTS, 20);
                config.SetConfig(FeedForwardDeepNeuralNetwork.NUMBER_OF_OUTPUTS, 3);
                config.SetConfig(FeedForwardDeepNeuralNetwork.NUMBER_OF_HIDDEN_LAYERS, numHiddenLayers);
                config.SetConfig(FeedForwardDeepNeuralNetwork.NUMBER_OF_HIDDEN_NEURONS_PER_LAYER, numNeuronsPerLayer);
                config.SetConfig(FeedForwardDeepNeuralNetwork.LOWER_LIMIT_WEIGHTS, -2.0);
                config.SetConfig(FeedForwardDeepNeuralNetwork.UPPER_LIMIT_WEIGHTS, 2.0);

                FeedForwardDeepNeuralNetwork ffnn = new FeedForwardDeepNeuralNetwork(config, new LogSigActivationFunction());
                ffnn.SetTrainingScheme(new BackPropagationDeepLearning(0.1, 0.9));

                ffnn.TrainOn(innds, epochs);

                innds.RefreshDataset();
                int[] result = ffnn.TestOnDataSet(innds);
                System.Console.WriteLine(result[0] + " right, " + result[1] + " wrong");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        internal static void backPropogationDemo()
        {
            try
            {
                System.Console.WriteLine(Util.ntimes("*", 100));
                System.Console.WriteLine(
                    "\n BackpropagationDemo  - Running BackProp on Iris data Set with {0} epochs of learning ",
                    epochs);
                System.Console.WriteLine(Util.ntimes("*", 100));

                DataSet animalDataSet = DataSetFactory.getAnimalDataSet();
                INumerizer numerizer = new AnimalDataSetNumerizer();
                NeuralNetworkDataSet innds = new IrisNeuralNetworkDataSet();

                innds.CreateExamplesFromDataSet(animalDataSet, numerizer);

                NeuralNetworkConfig config = new NeuralNetworkConfig();
                config.SetConfig(FeedForwardNeuralNetwork.NUMBER_OF_INPUTS, 20);
                config.SetConfig(FeedForwardNeuralNetwork.NUMBER_OF_OUTPUTS, 3);
                config.SetConfig(FeedForwardNeuralNetwork.NUMBER_OF_HIDDEN_NEURONS, numNeuronsPerLayer);
                config.SetConfig(FeedForwardNeuralNetwork.LOWER_LIMIT_WEIGHTS, -2.0);
                config.SetConfig(FeedForwardNeuralNetwork.UPPER_LIMIT_WEIGHTS, 2.0);

                FeedForwardNeuralNetwork ffnn = new FeedForwardNeuralNetwork(config);
                ffnn.SetTrainingScheme(new BackPropagationLearning(0.1, 0.9));

                ffnn.TrainOn(innds, epochs);

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
