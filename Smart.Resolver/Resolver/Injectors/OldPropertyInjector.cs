﻿namespace Smart.Resolver.Injectors
{
    using System;

    using Smart.Resolver.Bindings;
    using Smart.Resolver.Metadatas;

    /// <summary>
    ///
    /// </summary>
    public sealed class OldPropertyInjector : IOldInjector
    {
        public bool IsTarget(IKernel kernel, IBinding binding, OldTypeMetadata metadata, Type type)
        {
            return metadata.TargetProperties.Length > 0;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="kernel"></param>
        /// <param name="binding"></param>
        /// <param name="metadata"></param>
        /// <param name="instance"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public void Inject(IKernel kernel, IBinding binding, OldTypeMetadata metadata, object instance)
        {
            for (var i = 0; i < metadata.TargetProperties.Length; i++)
            {
                var property = metadata.TargetProperties[i];
                var parameter = binding.PropertyValues.GetParameter(property.Name);
                property.Setter(
                    instance,
                    parameter != null
                        ? parameter.Resolve(kernel)
                        : property.Constraint != null
                            ? kernel.Get(property.PropertyType, property.Constraint)
                            : kernel.Get(property.PropertyType));
            }
        }
    }
}
