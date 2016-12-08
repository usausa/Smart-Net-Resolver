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
        /// <summary>
        ///
        /// </summary>
        public Type TargetType { get; }

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

            TargetType = type;
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
            var metadata = metadataFactory.GetMetadata(TargetType);

            if (metadata.TargetConstructors.Count == 0)
            {
                throw new InvalidOperationException(
                    String.Format(CultureInfo.InvariantCulture, "No constructor avaiable. type = {0}", TargetType.Name));
            }

            for (var i = 0; i < metadata.TargetConstructors.Count; i++)
            {
                var constructor = metadata.TargetConstructors[i];

                bool result;
                var arguments = TryResolveParameters(kernel, binding, constructor, out result);
                if (result)
                {
                    var instance = constructor.Constructor.Invoke(arguments);

                    var pipeline = kernel.Components.Get<IInjectPipeline>();
                    pipeline?.Inject(kernel, binding, metadata, instance);

                    return instance;
                }
            }

            throw new InvalidOperationException(
                String.Format(CultureInfo.InvariantCulture, "Constructor parameter unresolved. type = {0}", TargetType.Name));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="kernel"></param>
        /// <param name="binding"></param>
        /// <param name="cm"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private static object[] TryResolveParameters(IKernel kernel, IBinding binding, ConstructorMetadata cm, out bool result)
        {
            var parameters = cm.Parameters;
            if (parameters.Count == 0)
            {
                result = true;
                return null;
            }

            var arguments = new object[parameters.Count];
            for (var i = 0; i < parameters.Count; i++)
            {
                var parameter = parameters[i];
                var pi = parameter.Parameter;

                // Constructor argument
                var argument = binding.GetConstructorArgument(pi.Name);
                if (argument != null)
                {
                    arguments[i] = argument.Resolve(kernel);
                    continue;
                }

                // Multiple
                if (parameter.ElementType != null)
                {
                    arguments[i] = ResolverHelper.ConvertArray(parameter.ElementType, kernel.ResolveAll(parameter.ElementType, cm.Constraints[i]));
                    continue;
                }

                // Resolve
                bool resolve;
                var obj = kernel.TryResolve(pi.ParameterType, cm.Constraints[i], out resolve);
                if (resolve)
                {
                    arguments[i] = obj;
                    continue;
                }

                // DefaultValue
                if (pi.HasDefaultValue)
                {
                    arguments[i] = pi.DefaultValue;
                }

                result = false;
                return null;
            }

            result = true;
            return arguments;
        }
    }
}
