using System;
using tvn.cosine.ai.learning.framework;
using tvn.cosine.ai.learning.neural; 

namespace TvnTestConsoleApp.demo.learning
{
    class BackPropogationDemo
    {
        public static void Main(params string[] args)
        {
            Console.WriteLine(tvn.cosine.ai.util.Util.ntimes("*", 100));
            Console.WriteLine("\n BackpropagationDemo  - Running BackProp on Iris data Set with 10 epochs of learning ");
            Console.WriteLine(tvn.cosine.ai.util.Util.ntimes("*", 100));

            backPropogationDemo();

            Console.WriteLine("Complete, press <ENTER> to quit");
            Console.ReadLine();
        }

        public static void backPropogationDemo()
        {
            DataSet irisDataSet = DataSetFactory.getIrisDataSet();
            Numerizer numerizer = new IrisDataSetNumerizer();
            NNDataSet innds = new IrisNNDataSet();

            innds.createExamplesFromDataSet(irisDataSet, numerizer);

            NNConfig config = new NNConfig();
            config.setConfig(FeedForwardNeuralNetwork.NUMBER_OF_INPUTS, 4);
            config.setConfig(FeedForwardNeuralNetwork.NUMBER_OF_OUTPUTS, 3);
            config.setConfig(FeedForwardNeuralNetwork.NUMBER_OF_HIDDEN_NEURONS, 6);
            config.setConfig(FeedForwardNeuralNetwork.LOWER_LIMIT_WEIGHTS, -2.0);
            config.setConfig(FeedForwardNeuralNetwork.UPPER_LIMIT_WEIGHTS, 2.0);

            FeedForwardNeuralNetwork ffnn = new FeedForwardNeuralNetwork(config);
            ffnn.setTrainingScheme(new BackPropLearning(0.1, 0.9));

            ffnn.trainOn(innds, 10);

            innds.refreshDataset();
            int[] result = ffnn.testOnDataSet(innds);
            Console.WriteLine(result[0] + " right, " + result[1] + " wrong");

        }
    }
}
