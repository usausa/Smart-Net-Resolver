namespace Smart.Resolver.Providers
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading;

    using Smart.Resolver.Bindings;
    using Smart.Resolver.Injectors;
    using Smart.Resolver.Metadatas;

    /// <summary>
    ///
    /// </summary>
    public class StandardProvider : IProvider
    {
        private volatile TypeMetadata metadata;

        private volatile IInjector[] injectors;

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public object Create(IKernel kernel, IBinding binding)
        {
            if (metadata == null)
            {
#pragma warning disable 420
                Interlocked.CompareExchange(ref metadata, kernel.Components.Get<IMetadataFactory>().GetMetadata(TargetType), null);
#pragma warning restore 420
            }

            if (injectors == null)
            {
#pragma warning disable 420
                Interlocked.CompareExchange(ref injectors, kernel.Components.GetAll<IInjector>().ToArray(), null);
#pragma warning restore 420
            }

            if (metadata.TargetConstructors.Length == 0)
            {
                throw new InvalidOperationException(
                    String.Format(CultureInfo.InvariantCulture, "No constructor avaiable. type = {0}", TargetType.Name));
            }

            for (var i = 0; i < metadata.TargetConstructors.Length; i++)
            {
                var constructor = metadata.TargetConstructors[i];

                bool result;
                var arguments = TryResolveParameters(kernel, binding, constructor, out result);
                if (result)
                {
                    var instance = constructor.Constructor.Invoke(arguments);

                    for (var j = 0; j < injectors.Length; j++)
                    {
                        injectors[j].Inject(kernel, binding, metadata, instance);
                    }

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
            if (parameters.Length == 0)
            {
                result = true;
                return null;
            }

            var arguments = new object[parameters.Length];
            for (var i = 0; i < arguments.Length; i++)
            {
                var parameter = parameters[i];
                var pi = parameter.Parameter;

                // Constructor argument
                var argument = binding.ConstructorArguments.GetParameter(pi.Name);
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
