namespace Example.WebApplication.Services
{
    using System.Threading;

    public class MetricsManager
    {
        private int counter;

        public int Counter => counter;

        public void Increment()
        {
            Interlocked.Add(ref counter, 1);
        }
    }
}
