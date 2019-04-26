namespace NewFactoryLib
{
    using System;

    public static class Scope
    {
        public static Func<IResolver, object> ToSingleton(IResolver resolver, Func<IResolver, object> factory)
        {
            var obj = factory(resolver);
            return r => obj;
        }
    }
}
