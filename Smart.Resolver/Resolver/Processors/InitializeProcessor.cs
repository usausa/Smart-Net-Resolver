namespace Smart.Resolver.Processors
{
    using System;

    public sealed class InitializeProcessor : IProcessor
    {
        private static readonly Type InitializableType = typeof(IInitializable);

        public Action<object> CreateProcessor(Type type, IKernel kernel)
        {
            if (!InitializableType.IsAssignableFrom(type))
            {
                return null;
            }

            return x => ((IInitializable)x).Initialize();
        }
    }
}
