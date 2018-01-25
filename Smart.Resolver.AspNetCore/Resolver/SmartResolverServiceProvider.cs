namespace Smart.Resolver
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
                // TODO
                var type = serviceType.GenericTypeArguments[0];
                return ConvertArray(type, resolver.GetAll(type, null));
            }

            return resolver.Get(serviceType);
        }

        public static Array ConvertArray(Type elementType, IEnumerable<object> source)
        {
            var sourceArray = source.ToArray();
            var array = Array.CreateInstance(elementType, sourceArray.Length);
            Array.Copy(sourceArray, 0, array, 0, sourceArray.Length);
            return array;
        }
    }
}
