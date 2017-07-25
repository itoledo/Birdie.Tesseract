using tvn.cosine.ai.learning.neural.api;

namespace tvn.cosine.ai.learning.neural
{
    public class HardLimitActivationFunction : IActivationFunction
    {
        public double Activation(double parameter)
        {

            if (parameter < 0.0)
            {
                return 0.0;
            }
            else
            {
                return 1.0;
            }
        }

        public double Deriv(double parameter)
        {
            return 0.0;
        }
    }
}
