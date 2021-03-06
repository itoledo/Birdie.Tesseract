﻿using tvn.cosine.exceptions;
using tvn.cosine.ai.learning.framework;
using tvn.cosine.ai.learning.neural;
using tvn.cosine.ai.learning.neural.api;
using tvn.cosine.ai.learning.neural.examples;
using tvn.cosine.ai.util;

namespace tvn_cosine.ai.demo.learning.chapter18
{
    public class BackPropogationDemo
    {
        internal static void Main(params string[] args)
        {
            System.Console.WriteLine(Util.ntimes("*", 100));
            System.Console.WriteLine("\n BackpropagationDemo  - Running BackProp on Iris data Set with 1000 epochs of learning ");
            System.Console.WriteLine(Util.ntimes("*", 100));
            backPropogationDemo();
        }

        internal static void backPropogationDemo()
        {
            try
            {
                DataSet irisDataSet = DataSetFactory.getIrisDataSet();
                INumerizer numerizer = new IrisDataSetNumerizer();
                NeuralNetworkDataSet innds = new IrisNeuralNetworkDataSet();

                innds.CreateExamplesFromDataSet(irisDataSet, numerizer);

                NeuralNetworkConfig config = new NeuralNetworkConfig();
                config.SetConfig(FeedForwardNeuralNetwork.NUMBER_OF_INPUTS, 4);
                config.SetConfig(FeedForwardNeuralNetwork.NUMBER_OF_OUTPUTS, 3);
                config.SetConfig(FeedForwardNeuralNetwork.NUMBER_OF_HIDDEN_NEURONS,
                        6);
                config.SetConfig(FeedForwardNeuralNetwork.LOWER_LIMIT_WEIGHTS, -2.0);
                config.SetConfig(FeedForwardNeuralNetwork.UPPER_LIMIT_WEIGHTS, 2.0);

                FeedForwardNeuralNetwork ffnn = new FeedForwardNeuralNetwork(config);
                ffnn.SetTrainingScheme(new BackPropagationLearning(0.1, 0.9));

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
