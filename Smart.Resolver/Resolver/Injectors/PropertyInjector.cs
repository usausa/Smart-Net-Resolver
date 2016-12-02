namespace Smart.Resolver.Injectors
{
    using Smart.Resolver.Bindings;
    using Smart.Resolver.Metadatas;

    /// <summary>
    ///
    /// </summary>
    public class PropertyInjector : IInjector
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="kernel"></param>
        /// <param name="binding"></param>
        /// <param name="metadata"></param>
        /// <param name="instance"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public void Inject(IKernel kernel, IBinding binding, TypeMetadata metadata, object instance)
        {
            if (metadata.TargetProperties.Count == 0)
            {
                return;
            }

            for (var i = 0; i < metadata.TargetProperties.Count; i++)
            {
                var property = metadata.TargetProperties[i];
                var parameter = binding.GetPropertyValue(property.Accessor.Name);
                var value = parameter != null ? parameter.Resolve(kernel) : kernel.Resolve(property.Accessor.Type, property.Constraint);
                property.Accessor.SetValue(instance, value);
            }
        }
    }
}
