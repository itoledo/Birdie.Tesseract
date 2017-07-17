namespace tvn.cosine.ai.learning.neural
{
    public class LogSigActivationFunction : ActivationFunction
    { 
        public double activation(double parameter)
        {

            return 1.0 / (1.0 + System.Math.Pow(System.Math.E, (-1.0 * parameter)));
        }

        public double deriv(double parameter)
        {
            // parameter = induced field
            // e == activation
            double e = 1.0 / (1.0 + System.Math.Pow(System.Math.E, (-1.0 * parameter)));
            return e * (1.0 - e);
        }
    }
}
