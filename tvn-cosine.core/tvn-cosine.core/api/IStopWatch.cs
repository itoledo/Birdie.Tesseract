namespace tvn.cosine.api
{
    public interface IStopWatch
    {
        long GetElapsedMilliseconds();
        long GetElapsedTicks();
        bool GetIsRunning(); 
        void Reset();
        void Restart();
        void Start();
        void Stop();
    }
}
