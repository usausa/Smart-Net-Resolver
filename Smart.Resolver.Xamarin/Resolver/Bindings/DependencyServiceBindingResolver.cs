namespace Smart.Resolver.Bindings
{
    using System;

    using Smart.Resolver.Providers;

    /// <summary>
    ///
    /// </summary>
    public class DependencyServiceBindingResolver : IBindingResolver
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IBinding Resolve(Type type)
        {
            if (!type.GetIsInterface())
            {
                return null;
            }

            return new Binding(type, new BindingMetadata())
            {
                Provider = new DependencyServiceProvider(type)
            };
        }
    }
}
