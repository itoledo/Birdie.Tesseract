using System;
using tvn.cosine.ai.learning.framework;
using tvn.cosine.ai.learning.neural;
using tvn.cosine.ai.util;

namespace TvnTestConsoleApp.demo.learning
{
    class PerceptronDemo
    {
        public static void Main(params string[] args)
        {
            Console.WriteLine(Util.ntimes("*", 100));
            Console.WriteLine("\n Perceptron Demo - Running Perceptron on Iris data Set with 10 epochs of learning ");
            Console.WriteLine(Util.ntimes("*", 100));

            perceptronDemo();

            Console.WriteLine("Complete, press <ENTER> to quit");
            Console.ReadLine();
        }

        public static void perceptronDemo()
        { 
                DataSet irisDataSet = DataSetFactory.getIrisDataSet();
                Numerizer numerizer = new IrisDataSetNumerizer();
                NNDataSet innds = new IrisNNDataSet();

                innds.createExamplesFromDataSet(irisDataSet, numerizer);

                Perceptron perc = new Perceptron(3, 4);

                perc.trainOn(innds, 10);

                innds.refreshDataset();
                int[] result = perc.testOnDataSet(innds);
                Console.WriteLine(result[0] + " right, " + result[1] + " wrong");
         
        }
    }
}
