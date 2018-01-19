namespace Smart.Resolver
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Microsoft.Extensions.DependencyInjection;

    public sealed class SmartResolverServiceProvider : IServiceProvider, ISupportRequiredService
    {
        private static readonly Type EnumerableType = typeof(IEnumerable<>);

        private readonly IResolver resolver;

        /// <summary>
        ///
        /// </summary>
        /// <param name="resolver"></param>
        public SmartResolverServiceProvider(IResolver resolver)
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
                return resolver.ResolveAll(serviceType.GenericTypeArguments[0], null).Select(x => x.Create());
            }

            if (required)
            {
                return resolver.Get(serviceType);
            }

            return resolver.TryGet(serviceType, out bool _);
        }
    }
}
