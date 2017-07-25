using tvn.cosine.ai.learning.neural.api;

namespace tvn.cosine.ai.learning.neural
{
    public class PureLinearActivationFunction : IActivationFunction
    { 
        public double Activation(double parameter)
        {
            return parameter;
        }

        public double Deriv(double parameter)
        {  
            return 1;
        }
    }
}
