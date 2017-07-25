﻿using tvn.cosine.ai.learning.neural.api;
using tvn.cosine.ai.util;
using tvn.cosine.ai.util.math;

namespace tvn.cosine.ai.learning.neural
{
    /// <summary>
    /// Artificial Intelligence A Modern Approach (3rd Edition): page 729 
    /// <para />
    /// Feed-forward networks are usually arranged in layers, such that each unit
    /// receives input only from units in the immediately preceding layer. 
    /// </summary>
    public class Layer
    {
        // vectors are represented by n * 1 matrices;
        private readonly Matrix weightMatrix;
        private readonly IActivationFunction activationFunction;

        Vector biasVector;
        Vector lastBiasUpdateVector; 
        Vector lastActivationValues;
        Vector lastInducedField;
        Matrix lastWeightUpdateMatrix;
        Matrix penultimateWeightUpdateMatrix;
        Vector penultimateBiasUpdateVector;
        Vector lastInput;

        public Layer(Matrix weightMatrix, Vector biasVector, IActivationFunction af)
        {
            activationFunction = af;
            this.weightMatrix = weightMatrix;
            lastWeightUpdateMatrix = new Matrix(weightMatrix.getRowDimension(),
                    weightMatrix.getColumnDimension());
            penultimateWeightUpdateMatrix = new Matrix(
                    weightMatrix.getRowDimension(),
                    weightMatrix.getColumnDimension());

            this.biasVector = biasVector;
            lastBiasUpdateVector = new Vector(biasVector.getRowDimension());
            penultimateBiasUpdateVector = new Vector(biasVector.getRowDimension());
        }

        public Layer(int numberOfNeurons, int numberOfInputs,
                double lowerLimitForWeights, double upperLimitForWeights,
                IActivationFunction af)
        {

            activationFunction = af;
            this.weightMatrix = new Matrix(numberOfNeurons, numberOfInputs);
            lastWeightUpdateMatrix = new Matrix(weightMatrix.getRowDimension(),
                    weightMatrix.getColumnDimension());
            penultimateWeightUpdateMatrix = new Matrix(
                    weightMatrix.getRowDimension(),
                    weightMatrix.getColumnDimension());

            this.biasVector = new Vector(numberOfNeurons);
            lastBiasUpdateVector = new Vector(biasVector.getRowDimension());
            penultimateBiasUpdateVector = new Vector(biasVector.getRowDimension());

            initializeMatrix(weightMatrix, lowerLimitForWeights,
                    upperLimitForWeights);
            initializeVector(biasVector, lowerLimitForWeights, upperLimitForWeights);
        }

        public Vector feedForward(Vector inputVector)
        {
            lastInput = inputVector;
            Matrix inducedField = weightMatrix.times(inputVector).plus(biasVector);

            Vector inducedFieldVector = new Vector(numberOfNeurons());
            for (int i = 0; i < numberOfNeurons(); ++i)
            {
                inducedFieldVector.setValue(i, inducedField.get(i, 0));
            }

            lastInducedField = inducedFieldVector.copyVector();
            Vector resultVector = new Vector(numberOfNeurons());
            for (int i = 0; i < numberOfNeurons(); ++i)
            {
                resultVector.setValue(i, activationFunction
                        .Activation(inducedFieldVector.getValue(i)));
            }
            // set the result as the last activation value
            lastActivationValues = resultVector.copyVector();
            return resultVector;
        }

        public Matrix getWeightMatrix()
        {
            return weightMatrix;
        }

        public Vector getBiasVector()
        {
            return biasVector;
        }

        public int numberOfNeurons()
        {
            return weightMatrix.getRowDimension();
        }

        public int numberOfInputs()
        {
            return weightMatrix.getColumnDimension();
        }

        public Vector getLastActivationValues()
        {
            return lastActivationValues;
        }

        public Vector getLastInducedField()
        {
            return lastInducedField;
        }

        public Matrix getLastWeightUpdateMatrix()
        {
            return lastWeightUpdateMatrix;
        }

        public void setLastWeightUpdateMatrix(Matrix m)
        {
            lastWeightUpdateMatrix = m;
        }

        public Matrix getPenultimateWeightUpdateMatrix()
        {
            return penultimateWeightUpdateMatrix;
        }

        public void setPenultimateWeightUpdateMatrix(Matrix m)
        {
            penultimateWeightUpdateMatrix = m;
        }

        public Vector getLastBiasUpdateVector()
        {
            return lastBiasUpdateVector;
        }

        public void setLastBiasUpdateVector(Vector v)
        {
            lastBiasUpdateVector = v;
        }

        public Vector getPenultimateBiasUpdateVector()
        {
            return penultimateBiasUpdateVector;
        }

        public void setPenultimateBiasUpdateVector(Vector v)
        {
            penultimateBiasUpdateVector = v;
        }

        public void updateWeights()
        {
            weightMatrix.plusEquals(lastWeightUpdateMatrix);
        }

        public void updateBiases()
        {
            Matrix biasMatrix = biasVector.plusEquals(lastBiasUpdateVector);
            Vector result = new Vector(biasMatrix.getRowDimension());
            for (int i = 0; i < biasMatrix.getRowDimension(); ++i)
            {
                result.setValue(i, biasMatrix.get(i, 0));
            }
            biasVector = result;
        }

        public Vector getLastInputValues()
        {

            return lastInput;

        }

        public IActivationFunction getActivationFunction()
        { 
            return activationFunction;
        }

        public void acceptNewWeightUpdate(Matrix weightUpdate)
        {
            //penultimate weightupdates maintained only to implement VLBP later
            setPenultimateWeightUpdateMatrix(getLastWeightUpdateMatrix());
            setLastWeightUpdateMatrix(weightUpdate);
        }

        public void acceptNewBiasUpdate(Vector biasUpdate)
        {
            setPenultimateBiasUpdateVector(getLastBiasUpdateVector());
            setLastBiasUpdateVector(biasUpdate);
        }

        public Vector errorVectorFrom(Vector target)
        {
            return target.minus(getLastActivationValues()); 
        }

        private static void initializeMatrix(Matrix aMatrix, double lowerLimit, double upperLimit)
        {
            for (int i = 0; i < aMatrix.getRowDimension(); ++i)
            {
                for (int j = 0; j < aMatrix.getColumnDimension(); j++)
                {
                    double random = Util.generateRandomDoubleBetween(lowerLimit, upperLimit);
                    aMatrix.set(i, j, random);
                }
            } 
        }

        private static void initializeVector(Vector aVector, double lowerLimit, double upperLimit)
        {
            for (int i = 0; i < aVector.size(); ++i)
            {

                double random = Util.generateRandomDoubleBetween(lowerLimit,
                        upperLimit);
                aVector.setValue(i, random);
            }
        }
    }
}
