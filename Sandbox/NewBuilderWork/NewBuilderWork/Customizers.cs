namespace NewBuilderWork;

using System;

using Smart.Resolver.Builders;

public interface IInitialize
{
    void Initialize();
}

public static class ActionBuilder
{
    public static Action<IResolver, object> Create1()
    {
        return (c, o) => ((IInitialize)o).Initialize();
    }

    public static Action<IResolver, object> Create2()
    {
        return (c, o) => System.Diagnostics.Debug.WriteLine("Called");
    }
}
