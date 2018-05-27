namespace Smart.Resolver.Processors
{
    using System;

    public sealed class InitializeProcessor : IProcessor
    {
        private static readonly Type InitializableType = typeof(IInitializable);

        public bool IsTarget(Type type)
        {
            return InitializableType.IsAssignableFrom(type);
        }

        public Action<object> CreateProcessor(IKernel kernel)
        {
            return x => ((IInitializable)x).Initialize();
        }
    }
}
