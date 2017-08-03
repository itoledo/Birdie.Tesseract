using tvn.cosine.api;
using tvn.cosine.exceptions;

namespace tvn.cosine.ai.deeplearning
{
    public class DeepNeuralNetwork
    {
        private int numInput;
        private int numHiddenA;
        private int numHiddenB;
        private int numOutput;

        private double[] inputs;

        private double[][] iaWeights;
        private double[][] abWeights;
        private double[][] boWeights;

        private double[] aBiases;
        private double[] bBiases;
        private double[] oBiases;

        private double[] aOutputs;
        private double[] bOutputs;
        private double[] outputs;

        private static IRandom rnd;

        public DeepNeuralNetwork(int numInput, int numHiddenA, int numHiddenB, int numOutput)
        {
            this.numInput = numInput;
            this.numHiddenA = numHiddenA;
            this.numHiddenB = numHiddenB;
            this.numOutput = numOutput;

            inputs = new double[numInput];

            iaWeights = makeMatrix(numInput, numHiddenA);
            abWeights = makeMatrix(numHiddenA, numHiddenB);
            boWeights = makeMatrix(numHiddenB, numOutput);

            aBiases = new double[numHiddenA];
            bBiases = new double[numHiddenB];
            oBiases = new double[numOutput];

            aOutputs = new double[numHiddenA];
            bOutputs = new double[numHiddenB];
            outputs = new double[numOutput];

            rnd = CommonFactory.CreateRandom();
            initialiseWeights();
        } 

        private static double[][] makeMatrix(int rows, int cols) // helper for ctor
        {
            double[][] result = new double[rows][];
            for (int r = 0; r < result.Length; ++r)
            {
                result[r] = new double[cols];
            }
            return result;
        } 

        private void initialiseWeights()
        {
            int numWeights = (numInput * numHiddenA) + numHiddenA +
                             (numHiddenA * numHiddenB) + numHiddenB +
                             (numHiddenB * numOutput) + numOutput;
            double[] weights = new double[numWeights];
            double lo = -0.01;
            double hi = 0.01;
            for (int i = 0; i < weights.Length; ++i)
            {
                weights[i] = (hi - lo) * rnd.NextDouble() + lo;
            }
            this.SetWeights(weights);
        } 

        public void SetWeights(double[] weights)
        {
            int numWeights = (numInput * numHiddenA) + numHiddenA +
                             (numHiddenA * numHiddenB) + numHiddenB +
                             (numHiddenB * numOutput) + numOutput;
            if (weights.Length != numWeights)
            {
                throw new Exception("Bad weights length");
            }

            int k = 0;

            for (int i = 0; i < numInput; ++i)
            {
                for (int j = 0; j < numHiddenA; ++j)
                {
                    iaWeights[i][j] = weights[k++];
                }
            }

            for (int i = 0; i < numHiddenA; ++i)
            {
                aBiases[i] = weights[k++];
            }

            for (int i = 0; i < numHiddenA; ++i)
            {
                for (int j = 0; j < numHiddenB; ++j)
                {
                    abWeights[i][j] = weights[k++];
                }
            }

            for (int i = 0; i < numHiddenB; ++i)
            {
                bBiases[i] = weights[k++];
            }

            for (int i = 0; i < numHiddenB; ++i)
            {
                for (int j = 0; j < numOutput; ++j)
                {
                    boWeights[i][j] = weights[k++];
                }
            }

            for (int i = 0; i < numOutput; ++i)
            {
                oBiases[i] = weights[k++];
            }
        }  

        public double[] ComputeOutputs(double[] xValues)
        {
            double[] aSums = new double[numHiddenA]; // hidden A nodes sums scratch array
            double[] bSums = new double[numHiddenB]; // hidden B nodes sums scratch array
            double[] oSums = new double[numOutput]; // output nodes sums

            for (int i = 0; i < xValues.Length; ++i) // copy x-values to inputs
            {
                this.inputs[i] = xValues[i];
            }

            for (int j = 0; j < numHiddenA; ++j)  // compute sum of (ia) weights * inputs
            {
                for (int i = 0; i < numInput; ++i)
                {
                    aSums[j] += this.inputs[i] * this.iaWeights[i][j]; // note +=
                }
            }

            for (int i = 0; i < numHiddenA; ++i)  // add biases to a sums
            {
                aSums[i] += this.aBiases[i];
            }

            for (int i = 0; i < numHiddenA; ++i)   // apply activation
            {
                this.aOutputs[i] = hyperTanFunction(aSums[i]); // hard-coded
            }

            for (int j = 0; j < numHiddenB; ++j)  // compute sum of (ab) weights * a outputs = local inputs
            {
                for (int i = 0; i < numHiddenA; ++i)
                {
                    bSums[j] += aOutputs[i] * this.abWeights[i][j]; // note +=
                }
            }

            for (int i = 0; i < numHiddenB; ++i)  // add biases to b sums
            {
                bSums[i] += this.bBiases[i];
            }

            for (int i = 0; i < numHiddenB; ++i)   // apply activation
            {
                this.bOutputs[i] = hyperTanFunction(bSums[i]); // hard-coded
            }

            for (int j = 0; j < numOutput; ++j)   // compute sum of (bo) weights * b outputs = local inputs
            {
                for (int i = 0; i < numHiddenB; ++i)
                {
                    oSums[j] += bOutputs[i] * boWeights[i][j];
                }
            }

            for (int i = 0; i < numOutput; ++i)  // add biases to input-to-hidden sums
            {
                oSums[i] += oBiases[i];
            }

            double[] softOut = softmax(oSums); // softmax activation does all outputs at once for efficiency
            System.Array.Copy(softOut, outputs, softOut.Length);

            double[] retResult = new double[numOutput]; // could define a GetOutputs method instead
            System.Array.Copy(this.outputs, retResult, retResult.Length);
            return retResult;
        } 

        private static double hyperTanFunction(double x)
        {
            if (x < -20.0)
            {
                return -1.0D; // approximation is correct to 30 decimals
            }
            else if (x > 20.0)
            {
                return 1.0D;
            }
            else
            {
                return System.Math.Tanh(x);
            }
        }  

        private static double[] softmax(double[] oSums)
        {
            // determine max output sum
            // does all output nodes at once so scale doesn't have to be re-computed each time
            double max = oSums[0];
            for (int i = 0; i < oSums.Length; ++i)
            {
                if (oSums[i] > max)
                {
                    max = oSums[i];
                }
            }

            // determine scaling factor -- sum of exp(each val - max)
            double scale = 0.0;
            for (int i = 0; i < oSums.Length; ++i)
            {
                scale += System.Math.Exp(oSums[i] - max);
            }

            double[] result = new double[oSums.Length];
            for (int i = 0; i < oSums.Length; ++i)
            {
                result[i] = System.Math.Exp(oSums[i] - max) / scale;
            }

            return result; // now scaled so that xi sum to 1.0
        }  
    }  
}
