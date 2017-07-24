using tvn.cosine.ai.common.exceptions;
using tvn.cosine.ai.learning.framework;
using tvn.cosine.ai.learning.neural;
using tvn.cosine.ai.util;

namespace tvn_cosine.ai.demo.learning.chapter18
{
    public class PerceptronDemo
    {
        static void Main(params string[] args)
        {
            System.Console.WriteLine(Util.ntimes("*", 100));
            System.Console.WriteLine("\n Perceptron Demo - Running Perceptron on Iris data Set with 10 epochs of learning ");
            System.Console.WriteLine(Util.ntimes("*", 100));
            perceptronDemo();
        }

        static void perceptronDemo()
        {
            try
            { 
                DataSet irisDataSet = DataSetFactory.getIrisDataSet();
                Numerizer numerizer = new IrisDataSetNumerizer();
                NNDataSet innds = new IrisNNDataSet();

                innds.createExamplesFromDataSet(irisDataSet, numerizer);

                Perceptron perc = new Perceptron(3, 4);

                perc.trainOn(innds, 10);

                innds.refreshDataset();
                int[] result = perc.testOnDataSet(innds);
                System.Console.WriteLine(result[0] + " right, " + result[1] + " wrong");
            }
            catch (Exception e)
            {
                throw e;
            } 
        }
    }
}
