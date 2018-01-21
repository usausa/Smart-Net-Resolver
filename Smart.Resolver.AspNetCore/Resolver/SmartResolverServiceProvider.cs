namespace Smart.Resolver
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Extensions.DependencyInjection;

    public sealed class SmartResolverServiceProvider : IServiceProvider, ISupportRequiredService
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
            return GetServiceInternal(serviceType, false);
        }

        public object GetRequiredService(Type serviceType)
        {
            return GetServiceInternal(serviceType, true);
        }

        private object GetServiceInternal(Type serviceType, bool required)
        {
            if (serviceType.IsGenericType && serviceType.GetGenericTypeDefinition() == EnumerableType)
            {
                return resolver.GetAll(serviceType.GenericTypeArguments[0], null);
            }

            if (required)
            {
                return resolver.Get(serviceType);
            }

            return resolver.TryGet(serviceType, out bool _);
        }
    }
}
