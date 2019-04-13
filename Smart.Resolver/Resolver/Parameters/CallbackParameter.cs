namespace Smart.Resolver.Parameters
{
    using System;

    public sealed class CallbackParameter : IParameter
    {
        private readonly Func<IResolver, object> factory;

        public CallbackParameter(Func<IResolver, object> factory)
        {
            this.factory = factory;
        }

        public object Resolve(IResolver resolver)
        {
            return factory(resolver);
        }
    }
}
