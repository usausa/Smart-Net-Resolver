namespace Smart.Resolver.Builders
{
    using System;
    using System.Reflection;

    public interface IFactoryBuilder
    {
        Func<IResolver, object> CreateFactory(ConstructorInfo ci, Func<IResolver, object>[] factories, Action<IResolver, object>[] actions);

        Func<IResolver, object> CreateArrayFactory(Type type, Func<IResolver, object>[] factories);
    }
}
