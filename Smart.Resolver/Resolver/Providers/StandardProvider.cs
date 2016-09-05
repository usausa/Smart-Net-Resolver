namespace Smart.Resolver.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Smart.Resolver.Bindings;
    using Smart.Resolver.Injectors;
    using Smart.Resolver.Metadatas;

    /// <summary>
    ///
    /// </summary>
    public class StandardProvider : IProvider
    {
        private static readonly Type EnumerableType = typeof(IEnumerable<>);

        private static readonly Type CollectionType = typeof(ICollection<>);

        private static readonly Type ListType = typeof(IList<>);

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
                var arguments = new object[constructor.GetParameters().Length];
                for (var i = 0; i < constructor.GetParameters().Length; i++)
                {
                    var pi = constructor.GetParameters()[i];
                    var parameter = binding.GetConstructorArgument(pi.Name);
                    if (parameter != null)
                    {
                        arguments[i] = parameter.Resolve(kernel);
                        continue;
                    }

                    if (pi.ParameterType.IsArray)
                    {
                        var elementType = pi.ParameterType.GetElementType();
                        arguments[i] = ConvertArray(elementType, kernel.ResolveAll(elementType, metadata.TargetConstructor.Constraints[i]));
                        continue;
                    }

                    if (pi.ParameterType.GetIsGenericType())
                    {
                        var genericType = pi.ParameterType.GetGenericTypeDefinition();
                        if ((genericType == EnumerableType) || (genericType == CollectionType) || (genericType == ListType))
                        {
                            var elementType = pi.ParameterType.GenericTypeArguments[0];
                            arguments[i] = ConvertArray(elementType, kernel.ResolveAll(elementType, metadata.TargetConstructor.Constraints[i]));
                            continue;
                        }
                    }

                    arguments[i] = kernel.Resolve(pi.ParameterType, metadata.TargetConstructor.Constraints[i]);
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="elementType"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        private static Array ConvertArray(Type elementType, IEnumerable<object> source)
        {
            var sourceArray = source.ToArray();
            var array = Array.CreateInstance(elementType, sourceArray.Length);
            Array.Copy(sourceArray, 0, array, 0, sourceArray.Length);
            return array;
        }
    }
}
