namespace Smart.Resolver
{
    using System;
    using System.Collections.Generic;

    public sealed class SmartResolverServiceProvider : IServiceProvider
    {
        private static readonly Type EnumerableType = typeof(IEnumerable<>);

        private readonly SmartResolver resolver;

        /// <summary>
        ///
        /// </summary>
        /// <param name="resolver"></param>
        public SmartResolverServiceProvider(SmartResolver resolver)
        {
            this.resolver = resolver;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType.IsGenericType && serviceType.GetGenericTypeDefinition() == EnumerableType)
            {
                return resolver.GetAll(serviceType.GenericTypeArguments[0], null);
            }

            return resolver.Get(serviceType);
        }
    }
}
