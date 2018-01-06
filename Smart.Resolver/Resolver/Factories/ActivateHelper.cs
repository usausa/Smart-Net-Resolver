namespace Smart.Resolver.Factories
{
    using Smart.Resolver.Bindings;
    using Smart.Resolver.Injectors;
    using Smart.Resolver.Metadatas;
    using Smart.Resolver.Processors;

    public sealed class ActivateHelper
    {
        private readonly IInjector[] injectors;

        private readonly IProcessor[] processors;

        private readonly IKernel kernel;

        private readonly IBinding binding;

        private readonly TypeMetadata metadata;

        public ActivateHelper(
            IInjector[] injectors,
            IProcessor[] processors,
            IKernel kernel,
            IBinding binding,
            TypeMetadata metadata)
        {
            this.injectors = injectors;
            this.processors = processors;
            this.kernel = kernel;
            this.binding = binding;
            this.metadata = metadata;
        }

        public void Process(object instance)
        {
            for (var i = 0; i < injectors.Length; i++)
            {
                injectors[i].Inject(kernel, binding, metadata, instance);
            }

            for (var i = 0; i < processors.Length; i++)
            {
                processors[i].Initialize(instance);
            }
        }
    }
}
