using tvn.cosine.ai.learning.neural.api;
using tvn.cosine.ai.util.math;

namespace tvn.cosine.ai.learning.neural
{
    public class BackPropagationLearning : INeuralNetworkTrainingScheme
    {
        private readonly double learningRate;
        private readonly double momentum;

        private Layer hiddenLayer;
        private Layer outputLayer;
        private LayerSensitivity hiddenSensitivity;
        private LayerSensitivity outputSensitivity;

        public BackPropagationLearning(double learningRate, double momentum)
        {
            this.learningRate = learningRate;
            this.momentum = momentum;
        }

        public void SetNeuralNetwork(IFunctionApproximator fapp)
        {
            FeedForwardNeuralNetwork ffnn = (FeedForwardNeuralNetwork)fapp;
            this.hiddenLayer = ffnn.GetHiddenLayer();
            this.outputLayer = ffnn.GetOutputLayer();
            this.hiddenSensitivity = new LayerSensitivity(hiddenLayer);
            this.outputSensitivity = new LayerSensitivity(outputLayer);
        }

        public Vector ProcessInput(FeedForwardNeuralNetwork network, 
                                   Vector input)
        { 
            hiddenLayer.FeedForward(input);
            outputLayer.FeedForward(hiddenLayer.GetLastActivationValues());
            return outputLayer.GetLastActivationValues();
        }

        public void ProcessError(FeedForwardNeuralNetwork network, 
                                 Vector error)
        {
            // TODO calculate total error somewhere
            // create Sensitivity Matrices
            outputSensitivity.SensitivityMatrixFromErrorMatrix(error);

            hiddenSensitivity.SensitivityMatrixFromSucceedingLayer(outputSensitivity);

            // calculate weight Updates
            CalculateWeightUpdates(outputSensitivity, hiddenLayer.GetLastActivationValues(), learningRate, momentum);
            CalculateWeightUpdates(hiddenSensitivity, hiddenLayer.GetLastInputValues(), learningRate, momentum);

            // calculate Bias Updates
            CalculateBiasUpdates(outputSensitivity, learningRate, momentum);
            CalculateBiasUpdates(hiddenSensitivity, learningRate, momentum);

            // update weightsAndBiases
            outputLayer.UpdateWeights();
            outputLayer.UpdateBiases();

            hiddenLayer.UpdateWeights();
            hiddenLayer.UpdateBiases();

        }

        public Matrix CalculateWeightUpdates(LayerSensitivity layerSensitivity, 
                                             Vector previousLayerActivationOrInput,
                                             double alpha, 
                                             double momentum)
        {
            Layer layer = layerSensitivity.GetLayer();
            Matrix activationTranspose 
                = previousLayerActivationOrInput.transpose();
            Matrix momentumLessUpdate 
                = layerSensitivity.GetSensitivityMatrix()
                                  .times(activationTranspose)
                                  .times(alpha)
                                  .times(-1.0);
            Matrix updateWithMomentum 
                = layer.GetLastWeightUpdateMatrix()
                       .times(momentum)
                       .plus(momentumLessUpdate.times(1.0 - momentum));
            layer.AcceptNewWeightUpdate(updateWithMomentum.copy());

            return updateWithMomentum;
        }

        public static Matrix CalculateWeightUpdates(LayerSensitivity layerSensitivity, 
                                                    Vector previousLayerActivationOrInput,
                                                    double alpha)
        {
            Layer layer = layerSensitivity.GetLayer();
            Matrix activationTranspose = previousLayerActivationOrInput.transpose();
            Matrix weightUpdateMatrix
                = layerSensitivity.GetSensitivityMatrix()
                                  .times(activationTranspose)
                                  .times(alpha)
                                  .times(-1.0);
            layer.AcceptNewWeightUpdate(weightUpdateMatrix.copy());

            return weightUpdateMatrix;
        }

        public Vector CalculateBiasUpdates(LayerSensitivity layerSensitivity,
                                           double alpha, 
                                           double momentum)
        {
            Layer layer = layerSensitivity.GetLayer();
            Matrix biasUpdateMatrixWithoutMomentum = layerSensitivity.GetSensitivityMatrix().times(alpha).times(-1.0);

            Matrix biasUpdateMatrixWithMomentum
                = layer.GetLastBiasUpdateVector()
                       .times(momentum)
                       .plus(biasUpdateMatrixWithoutMomentum
                       .times(1.0 - momentum));
            Vector result = new Vector(biasUpdateMatrixWithMomentum.getRowDimension());
            for (int i = 0; i < biasUpdateMatrixWithMomentum.getRowDimension(); ++i)
            {
                result.setValue(i, biasUpdateMatrixWithMomentum.get(i, 0));
            }
            layer.AcceptNewBiasUpdate(result.copyVector());
            return result;
        }

        public static Vector CalculateBiasUpdates(LayerSensitivity layerSensitivity, 
                                                  double alpha)
        {
            Layer layer = layerSensitivity.GetLayer();
            Matrix biasUpdateMatrix 
                = layerSensitivity.GetSensitivityMatrix()
                                  .times(alpha)
                                  .times(-1.0);

            Vector result = new Vector(biasUpdateMatrix.getRowDimension());
            for (int i = 0; i < biasUpdateMatrix.getRowDimension(); ++i)
            {
                result.setValue(i, biasUpdateMatrix.get(i, 0));
            }
            layer.AcceptNewBiasUpdate(result.copyVector());
            return result;
        }
    }
}
