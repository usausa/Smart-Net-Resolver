﻿namespace Smart.Resolver.Bindings
{
    using System;
    using System.Collections.Generic;

    using Smart.Resolver.Providers;

    /// <summary>
    ///
    /// </summary>
    public class DependencyServiceBindingResolver : IBindingResolver
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IEnumerable<IBinding> Resolve(IResolverContext context, Type type)
        {
            if (!type.GetIsInterface())
            {
                return null;
            }

            return new[]
            {
                new Binding(type, new BindingMetadata())
                {
                    Provider = new DependencyServiceProvider(type)
                }
            };
        }
    }
}
