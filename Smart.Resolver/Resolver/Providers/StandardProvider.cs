namespace Smart.Resolver.Providers
{
    using System;
    using System.Globalization;

    using Smart.Resolver.Bindings;
    using Smart.Resolver.Injectors;
    using Smart.Resolver.Metadatas;

    /// <summary>
    ///
    /// </summary>
    public class StandardProvider : IProvider
    {
        private readonly Type type;

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        public StandardProvider(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            this.type = type;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="kernel"></param>
        /// <param name="binding"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public object Create(IKernel kernel, IBinding binding)
        {
            var metadataFactory = kernel.Components.Get<IMetadataFactory>();
            var metadata = metadataFactory.GetMetadata(type);
            var constructor = metadata.TargetConstructor.Constructor;
            if (constructor == null)
            {
                throw new InvalidOperationException(
                    String.Format(CultureInfo.InvariantCulture, "No constructor avaiable. type = {0}", type.Name));
            }

            object instance;
            if (constructor.GetParameters().Length > 0)
            {
                // TODO
                var arguments = new object[constructor.GetParameters().Length];
                for (var i = 0; i < constructor.GetParameters().Length; i++)
                {
                    var pi = constructor.GetParameters()[i];
                    var parameter = binding.GetConstructorArgument(pi.Name);
                    arguments[i] = parameter != null
                        ? parameter.Resolve(kernel)
                        : kernel.Resolve(pi.ParameterType, metadata.TargetConstructor.Constraints[i]);
                }

                instance = constructor.Invoke(arguments);
            }
            else
            {
                instance = constructor.Invoke(null);
            }

            var pipeline = kernel.Components.Get<IInjectPipeline>();
            pipeline?.Inject(kernel, binding, metadata, instance);

            return instance;
        }
    }
}
