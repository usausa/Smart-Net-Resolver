namespace Smart.Resolver.Builders
{
    using System;
    using System.Reflection;

    public sealed class ReflectionBuilder : IBuilder
    {
        public object CreateFactory(ConstructorInfo ci, object[] factories, object[] actions)
        {
            // TODO
            return null;
        }

        public object CreateArrayFactory(Type type, object[] factories)
        {
            throw new NotImplementedException();
        }
    }
}
