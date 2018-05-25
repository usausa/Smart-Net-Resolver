namespace Smart.Resolver.Injectors
{
    using System;

    using Smart.Resolver.Bindings;
    using Smart.Resolver.Metadatas;

    using Xamarin.Forms;

    /// <summary>
    ///
    /// </summary>
    public sealed class BindingContextInjector : IOldInjector
    {
        private static readonly Type BindableObjectType = typeof(BindableObject);

        /// <summary>
        ///
        /// </summary>
        /// <param name="kernel"></param>
        /// <param name="binding"></param>
        /// <param name="metadata"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool IsTarget(IKernel kernel, IBinding binding, TypeMetadata metadata, Type type)
        {
            return BindableObjectType.IsAssignableFrom(type);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="kernel"></param>
        /// <param name="binding"></param>
        /// <param name="metadata"></param>
        /// <param name="instance"></param>
        public void Inject(IKernel kernel, IBinding binding, TypeMetadata metadata, object instance)
        {
            // TODO
        }
    }
}
