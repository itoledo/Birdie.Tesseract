using tvn.cosine.ai.deeplearning;

namespace tvn_cosine.ai.demo.deeplearning
{
    public class DeepLearningExample
    {
        static int numInput = 3;
        static int numHiddenA = 4;
        static int numHiddenB = 5;
        static int numOutput = 2;

        static void Main(string[] args)
        {
            System.Console.WriteLine("\nBegin Deep Neural Network input-output demo");

            System.Console.WriteLine("\nCreating a {0}-{1}-{2}-{3} neural network", 
                numInput, numHiddenA, numHiddenB, numOutput);

            DeepNeuralNetwork dnn = new DeepNeuralNetwork(numInput, numHiddenA, numHiddenB, numOutput);

            double[] weights = new double[] { 0.01, 0.02, 0.03, 0.04, 0.05, 0.06, 0.07, 0.08, 0.09, 0.10,
                                              0.11, 0.12, 0.13, 0.14, 0.15, 0.16, 0.17, 0.18, 0.19, 0.20,
                                              0.21, 0.22, 0.23, 0.24, 0.25, 0.26, 0.27, 0.28, 0.29, 0.30,
                                              0.31, 0.32, 0.33, 0.34, 0.35, 0.36, 0.37, 0.38, 0.39, 0.40,
                                              0.41, 0.42, 0.43, 0.44, 0.45, 0.46, 0.47, 0.48, 0.49, 0.50,
                                              0.51, 0.52, 0.53 };

            dnn.SetWeights(weights);

            double[] xValues = new double[] { 1.0, 2.0, 3.0 };

            System.Console.WriteLine("\nDummy weights and bias values are:");
            ShowVector(weights, 10, 2, true);

            System.Console.WriteLine("\nDummy inputs are:");
            ShowVector(xValues, 3, 1, true);

            double[] yValues = dnn.ComputeOutputs(xValues);

            System.Console.WriteLine("\nComputed outputs are:");
            ShowVector(yValues, 2, 4, true);


            System.Console.WriteLine("\nEnd deep neural network input-output demo\n");
            System.Console.ReadLine();

        } // Main

        static public void ShowVector(double[] vector, int valsPerRow, int decimals, bool newLine)
        {
            for (int i = 0; i < vector.Length; ++i)
            {
                if (i % valsPerRow == 0)
                {
                    System.Console.WriteLine("");
                }
                System.Console.Write(vector[i].ToString("F" + decimals).PadLeft(decimals + 4) + " ");
            }
            if (newLine == true)
            {
                System.Console.WriteLine("");
            }
        }
    }
}
