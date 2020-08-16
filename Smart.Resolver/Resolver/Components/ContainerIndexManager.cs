namespace Smart.Resolver.Components
{
    internal sealed class ContainerIndexManager
    {
        private readonly object sync = new object();

        private int index;

        public int Acquire()
        {
            lock (sync)
            {
                return index++;
            }
        }
    }
}
