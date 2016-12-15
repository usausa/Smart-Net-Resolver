namespace Smart.Resolver.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Smart.Resolver.Bindings;
    using Smart.Resolver.Providers;

    /// <summary>
    ///
    /// </summary>
    public class SelfMissingHandler : IMissingHandler
    {
        private static readonly Type StringType = typeof(string);

        /// <summary>
        ///
        /// </summary>
        /// <param name="table"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IEnumerable<IBinding> Handle(IBindingTable table, Type type)
        {
            if (type.GetIsInterface() || type.GetIsAbstract() || type.GetIsValueType() || (type == StringType) ||
                type.GetContainsGenericParameters())
            {
                return Enumerable.Empty<IBinding>();
            }

            return new[]
            {
                new Binding(type, new StandardProvider(type))
            };
        }
    }
}
