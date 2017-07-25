namespace tvn.cosine.ai.util.api
{
    public interface IToDoubleFunction<T>
    {
        double applyAsDouble(T value);
    }
}
