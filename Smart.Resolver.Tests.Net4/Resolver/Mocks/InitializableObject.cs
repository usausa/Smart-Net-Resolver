namespace Smart.Resolver.Mocks
{
    public class InitializableObject : IInitializable
    {
        public int InitializedCount { get; private set; }

        public void Initialize()
        {
            InitializedCount++;
        }
    }
}
