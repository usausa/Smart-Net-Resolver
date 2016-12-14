﻿namespace Smart.Resolver.Bindings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Smart.Resolver.Providers;

    /// <summary>
    ///
    /// </summary>
    public class SelfBindingResolver : IBindingResolver
    {
        private static readonly Type StringType = typeof(string);

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IEnumerable<IBinding> Resolve(IResolverContext context, Type type)
        {
            if (type.GetIsInterface() || type.GetIsAbstract() || type.GetIsValueType() || (type == StringType) ||
                type.GetContainsGenericParameters())
            {
                return Enumerable.Empty<IBinding>();
            }

            return new[]
            {
                new Binding(type, new BindingMetadata())
                {
                    Provider = new StandardProvider(type)
                }
            };
        }
    }
}