namespace Smart.Resolver.Processors
{
    using System;

    public sealed class InitializeProcessor : IProcessor
    {
        private static readonly Type InitializableType = typeof(IInitializable);

        public int Order { get; }

        public InitializeProcessor()
            : this(Int32.MinValue)
        {
        }

        public InitializeProcessor(int order)
        {
            Order = order;
        }

        public Action<IKernel, object> CreateProcessor(Type type, IKernel kernel)
        {
            if (!InitializableType.IsAssignableFrom(type))
            {
                return null;
            }

            return (k, x) => ((IInitializable)x).Initialize();
        }
    }
}
