namespace Smart.Resolver.Builders
{
    using System;
    using System.Reflection;

    public interface IFactoryBuilder
    {
        object CreateFactory(ConstructorInfo ci, object[] factories, object[] actions);

        object CreateArrayFactory(Type type, object[] factories);
    }
}
