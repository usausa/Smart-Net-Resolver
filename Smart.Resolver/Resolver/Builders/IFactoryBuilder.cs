namespace Smart.Resolver.Builders
{
    using System;
    using System.Reflection;

    public interface IFactoryBuilder
    {
        object CreateFactory(ConstructorInfo ci, Func<IResolver, object>[] factories, Action<IResolver, object>[] actions);

        object CreateArrayFactory(Type type, Func<IResolver, object>[] factories);
    }
}
