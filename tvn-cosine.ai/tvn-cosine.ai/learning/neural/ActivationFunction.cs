namespace tvn.cosine.ai.learning.neural
{
    public interface ActivationFunction
    {
        double activation(double parameter); 
        double deriv(double parameter);
    }
}
