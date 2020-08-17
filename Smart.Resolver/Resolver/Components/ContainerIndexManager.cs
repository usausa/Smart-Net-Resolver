namespace Smart.Resolver.Components
{
    internal static class ContainerIndexManager
    {
        private static readonly object Sync = new object();

        private static int index;

        public static int Acquire()
        {
            lock (Sync)
            {
                return index++;
            }
        }
    }
}
