namespace Smart.Resolver.Scopes
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Smart.ComponentModel;
    using Smart.Resolver.Components;

    public sealed class SingletonScope : IScope, IDisposable
    {
        [AllowNull]
        private object value;

        private Func<IResolver, object>? objectFactory;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Ignore")]
        public SingletonScope(ComponentContainer components)
        {
            components.Get<DisposableStorage>().Add(this);
        }

        public void Dispose()
        {
            (value as IDisposable)?.Dispose();
        }

        public IScope Copy(ComponentContainer components)
        {
            return new SingletonScope(components);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Ignore")]
        public Func<IResolver, object> Create(Func<object> factory)
        {
            if (objectFactory is null)
            {
                value = factory();
                objectFactory = _ => value;
            }

            return objectFactory;
        }
    }
}
