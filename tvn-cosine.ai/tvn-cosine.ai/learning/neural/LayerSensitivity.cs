using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.collections.api;
using tvn.cosine.ai.util.math;

namespace tvn.cosine.ai.learning.neural
{
    /// <summary> 
    /// Contains sensitivity matrices and related calculations for each layer.
    /// Used for backprop learning
    /// </summary>
    public class LayerSensitivity
    {
        private readonly Layer layer;

        Matrix sensitivityMatrix;

        public LayerSensitivity(Layer layer)
        {
            Matrix weightMatrix = layer.GetWeightMatrix();
            this.sensitivityMatrix = new Matrix(weightMatrix.getRowDimension(),
                                                weightMatrix.getColumnDimension());
            this.layer = layer;
        }

        public Matrix GetSensitivityMatrix()
        {
            return sensitivityMatrix;
        }

        public Matrix SensitivityMatrixFromErrorMatrix(Vector errorVector)
        {
            Matrix derivativeMatrix = CreateDerivativeMatrix(layer.GetLastInducedField());
            Matrix calculatedSensitivityMatrix = derivativeMatrix.times(errorVector)
                                                                 .times(-2.0);
            sensitivityMatrix = calculatedSensitivityMatrix.copy();
            return calculatedSensitivityMatrix;
        }

        public Matrix SensitivityMatrixFromSucceedingLayer(LayerSensitivity nextLayerSensitivity)
        {
            Layer nextLayer = nextLayerSensitivity.GetLayer();
            Matrix derivativeMatrix = CreateDerivativeMatrix(layer.GetLastInducedField());
            Matrix weightTranspose = nextLayer.GetWeightMatrix()
                                              .transpose();
            Matrix calculatedSensitivityMatrix
                = derivativeMatrix.times(weightTranspose)
                                  .times(nextLayerSensitivity.GetSensitivityMatrix());
            sensitivityMatrix = calculatedSensitivityMatrix.copy();
            return sensitivityMatrix;
        }

        public Layer GetLayer()
        {
            return layer;
        }

        private Matrix CreateDerivativeMatrix(Vector lastInducedField)
        {
            ICollection<double> lst = CollectionFactory.CreateQueue<double>();
            for (int i = 0; i < lastInducedField.size(); ++i)
            {
                lst.Add(layer.GetActivationFunction()
                             .Deriv(lastInducedField.getValue(i)));
            }
            return Matrix.createDiagonalMatrix(lst);
        }
    }
}
