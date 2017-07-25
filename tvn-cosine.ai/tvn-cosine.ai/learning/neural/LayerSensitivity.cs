﻿using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.collections.api;
using tvn.cosine.ai.util.math;

namespace tvn.cosine.ai.learning.neural
{
    public class LayerSensitivity
    {
        /// <summary> 
        /// Contains sensitivity matrices and related calculations for each layer.
        /// Used for backprop learning
        /// </summary>
        private Matrix sensitivityMatrix;
        private readonly Layer layer;

        public LayerSensitivity(Layer layer)
        {
            Matrix weightMatrix = layer.getWeightMatrix();
            this.sensitivityMatrix = new Matrix(weightMatrix.getRowDimension(),
                    weightMatrix.getColumnDimension());
            this.layer = layer;

        }

        public Matrix getSensitivityMatrix()
        {
            return sensitivityMatrix;
        }

        public Matrix sensitivityMatrixFromErrorMatrix(Vector errorVector)
        {
            Matrix derivativeMatrix = createDerivativeMatrix(layer.getLastInducedField());
            Matrix calculatedSensitivityMatrix = derivativeMatrix.times(errorVector).times(-2.0);
            sensitivityMatrix = calculatedSensitivityMatrix.copy();
            return calculatedSensitivityMatrix;
        }

        public Matrix sensitivityMatrixFromSucceedingLayer(LayerSensitivity nextLayerSensitivity)
        {
            Layer nextLayer = nextLayerSensitivity.getLayer();
            Matrix derivativeMatrix = createDerivativeMatrix(layer.getLastInducedField());
            Matrix weightTranspose = nextLayer.getWeightMatrix().transpose();
            Matrix calculatedSensitivityMatrix
                = derivativeMatrix.times(weightTranspose).times(nextLayerSensitivity.getSensitivityMatrix());
            sensitivityMatrix = calculatedSensitivityMatrix.copy();
            return sensitivityMatrix;
        }

        public Layer getLayer()
        {
            return layer;
        }
         
        private Matrix createDerivativeMatrix(Vector lastInducedField)
        {
            ICollection<double> lst = CollectionFactory.CreateQueue<double>();
            for (int i = 0; i < lastInducedField.size(); ++i)
            {
                lst.Add(layer.getActivationFunction().Deriv(
                        lastInducedField.getValue(i)));
            }
            return Matrix.createDiagonalMatrix(lst);
        }
    }
}
