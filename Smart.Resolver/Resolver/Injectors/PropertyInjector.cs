namespace Smart.Resolver.Injectors
{
    using System;

    using Smart.Resolver.Bindings;

    public sealed class PropertyInjector : IInjector
    {
        public Action<object> CreateInjector(Type type, IKernel kernel, IBinding binding)
        {
            // TODO prop process

            throw new NotImplementedException();
        }
    }
}
