namespace Smart.Resolver.Bindings
{
    using System;
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
        /// <param name="type"></param>
        /// <returns></returns>
        public IBinding Resolve(Type type)
        {
            if (type.GetIsInterface() || type.GetIsAbstract() || type.GetIsValueType() || (type == StringType) ||
                type.GetContainsGenericParameters())
            {
                return null;
            }

            return new Binding(type, new BindingMetadata())
            {
                Provider = new StandardProvider(type)
            };
        }
    }
}
