using tvn.cosine.api;

namespace tvn.cosine.io.api
{
    public interface ITextReader : IDisposable
    {
        void Close();
        int Peek();
        int Read();
        string ReadLine();
        string ReadToEnd();
    }
}
