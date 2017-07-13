namespace tvn.cosine.ai.learning.neural
{
    /**
     * @author Ravi Mohan
     * 
     */
    public class PureLinearActivationFunction : ActivationFunction
    { 
        public double activation(double parameter)
        {
            return parameter;
        }

        public double deriv(double parameter)
        { 
            return 1;
        }
    }
}
