namespace Smart.Resolver.Scopes
{
    using System;

    using Smart.ComponentModel;
    using Smart.Resolver.Bindings;
    using Smart.Resolver.Components;

    public sealed class SingletonScope : IScope, IDisposable
    {
        private object value;

        private Func<object> objectFactory;

        public SingletonScope(IComponentContainer container)
        {
            container.Get<DisposableStorage>().Add(this);
        }

        public void Dispose()
        {
            (value as IDisposable)?.Dispose();
        }

        public IScope Copy(IComponentContainer components)
        {
            return new SingletonScope(components);
        }

        public Func<object> Create(IKernel kernel, IBinding binding, Func<object> factory)
        {
            if (objectFactory is null)
            {
                value = factory();
                objectFactory = () => value;
            }

            return objectFactory;
        }
    }
}
