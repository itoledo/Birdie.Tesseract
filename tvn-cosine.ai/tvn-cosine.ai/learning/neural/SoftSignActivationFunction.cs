using tvn.cosine.ai.learning.neural.api;

namespace tvn.cosine.ai.learning.neural
{
    public class SoftSignActivationFunction : IActivationFunction
    {
        public double Activation(double parameter)
        {
            return parameter / (1.0 + System.Math.Abs(parameter));
        }

        public double Deriv(double parameter)
        {
            return 1 / System.Math.Pow(1.0 + System.Math.Abs(parameter), 2);
        }
    }
}
