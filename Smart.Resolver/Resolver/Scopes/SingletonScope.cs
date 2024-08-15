namespace Smart.Resolver.Scopes;

using Smart.ComponentModel;
using Smart.Resolver.Components;

public sealed class SingletonScope : IScope, IDisposable
{
    private object? value;

    private Func<IResolver, object>? objectFactory;

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

    public Func<IResolver, object> Create(Func<object> factory)
    {
        if (objectFactory is null)
        {
            value = factory();
            var holder = new SingletonHolder(value);
            objectFactory = holder.Resolve;
        }

        return objectFactory;
    }

    private sealed class SingletonHolder
    {
        private readonly object value;

        public SingletonHolder(object value)
        {
            this.value = value;
        }

#pragma warning disable IDE0060
        public object Resolve(IResolver resolver) => value;
#pragma warning restore IDE0060
    }
}
