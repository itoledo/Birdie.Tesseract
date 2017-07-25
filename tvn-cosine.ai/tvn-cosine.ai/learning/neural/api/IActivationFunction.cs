namespace tvn.cosine.ai.learning.neural.api
{
    public interface IActivationFunction
    {
        double Activation(double parameter); 
        double Deriv(double parameter);
    }
}
