namespace tvn.cosine.ai.probability.proposition
{
    public interface DerivedProposition<T> : SentenceProposition<T>
    {
       string getDerivedName();
    }
}
