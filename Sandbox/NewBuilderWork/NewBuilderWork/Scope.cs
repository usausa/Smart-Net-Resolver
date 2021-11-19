namespace NewBuilderWork;

using System;

using Smart.Resolver.Builders;

public static class Scope
{
    public static Func<IResolver, object> ToSingleton(IResolver resolver, Func<IResolver, object> factory)
    {
        var obj = factory(resolver);
        return r => obj;
    }
}
