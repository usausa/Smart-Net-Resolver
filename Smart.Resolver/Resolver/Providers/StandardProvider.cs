namespace Smart.Resolver.Providers
{
    using System;
    using System.Globalization;
    using System.Linq;

    using Smart.ComponentModel;
    using Smart.Reflection;
    using Smart.Resolver.Bindings;
    using Smart.Resolver.Factories;
    using Smart.Resolver.Injectors;
    using Smart.Resolver.Metadatas;
    using Smart.Resolver.Processors;

    /// <summary>
    ///
    /// </summary>
    public sealed class StandardProvider : IProvider
    {
        private readonly IInjector[] injectors;

        private readonly IProcessor[] processors;

        private readonly IActivatorFactory activatorFactory;

        private readonly IArrayOperatorFactory arrayOperatorFactory;

        private readonly TypeMetadata metadata;

        /// <summary>
        ///
        /// </summary>
        public Type TargetType { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <param name="components"></param>
        public StandardProvider(Type type, IComponentContainer components)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (components == null)
            {
                throw new ArgumentNullException(nameof(components));
            }

            TargetType = type;
            injectors = components.GetAll<IInjector>().ToArray();
            processors = components.GetAll<IProcessor>().ToArray();
            activatorFactory = components.Get<IActivatorFactory>();
            arrayOperatorFactory = components.Get<IArrayOperatorFactory>();
            metadata = components.Get<IMetadataFactory>().GetMetadata(TargetType);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="components"></param>
        /// <returns></returns>
        public IProvider Copy(IComponentContainer components)
        {
            return new StandardProvider(TargetType, components);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="kernel"></param>
        /// <param name="binding"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public IObjectFactory CreateFactory(IKernel kernel, IBinding binding)
        {
            var constructor = FindBestConstructor(kernel, binding);
            var argumentFactories = ResolveArgumentsFactories(kernel, binding, constructor);
            var activator = activatorFactory.CreateActivator(constructor.Constructor);
            var helper = injectors.Length > 0 || processors.Length > 0
                ? new ActivateHelper(injectors, processors, kernel, binding, metadata)
                : null;

            switch (argumentFactories.Length)
            {
                case 0:
                    return new ActivatorObjectFactory0(
                        activator,
                        helper);
                case 1:
                    return new ActivatorObjectFactory1(
                        activator,
                        helper,
                        argumentFactories[0]);
                case 2:
                    return new ActivatorObjectFactory2(
                        activator,
                        helper,
                        argumentFactories[0],
                        argumentFactories[1]);
                case 3:
                    return new ActivatorObjectFactory3(
                        activator,
                        helper,
                        argumentFactories[0],
                        argumentFactories[1],
                        argumentFactories[2]);
                case 4:
                    return new ActivatorObjectFactory4(
                        activator,
                        helper,
                        argumentFactories[0],
                        argumentFactories[1],
                        argumentFactories[2],
                        argumentFactories[3]);
                case 5:
                    return new ActivatorObjectFactory5(
                        activator,
                        helper,
                        argumentFactories[0],
                        argumentFactories[1],
                        argumentFactories[2],
                        argumentFactories[3],
                        argumentFactories[4]);
                case 6:
                    return new ActivatorObjectFactory6(
                        activator,
                        helper,
                        argumentFactories[0],
                        argumentFactories[1],
                        argumentFactories[2],
                        argumentFactories[3],
                        argumentFactories[4],
                        argumentFactories[5]);
                case 7:
                    return new ActivatorObjectFactory7(
                        activator,
                        helper,
                        argumentFactories[0],
                        argumentFactories[1],
                        argumentFactories[2],
                        argumentFactories[3],
                        argumentFactories[4],
                        argumentFactories[5],
                        argumentFactories[6]);
                case 8:
                    return new ActivatorObjectFactory8(
                        activator,
                        helper,
                        argumentFactories[0],
                        argumentFactories[1],
                        argumentFactories[2],
                        argumentFactories[3],
                        argumentFactories[4],
                        argumentFactories[5],
                        argumentFactories[6],
                        argumentFactories[7]);
                default:
                    return new ActivatorObjectFactory(
                        activator,
                        helper,
                        argumentFactories);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="kernel"></param>
        /// <param name="binding"></param>
        /// <returns></returns>
        private ConstructorMetadata FindBestConstructor(IKernel kernel, IBinding binding)
        {
            if (metadata.TargetConstructors.Length == 0)
            {
                throw new InvalidOperationException(
                    String.Format(CultureInfo.InvariantCulture, "No constructor avaiable. type = {0}", TargetType.Name));
            }

            for (var i = 0; i < metadata.TargetConstructors.Length; i++)
            {
                var match = true;
                var cm = metadata.TargetConstructors[i];

                var parameters = cm.Parameters;
                for (var j = 0; j < parameters.Length; j++)
                {
                    var parameter = parameters[j];
                    var pi = parameter.Parameter;

                    // Constructor argument
                    if (binding.ConstructorArguments.GetParameter(pi.Name) != null)
                    {
                        continue;
                    }

                    // Multiple
                    if (parameter.ElementType != null)
                    {
                        continue;
                    }

                    // Resolve
                    if (kernel.CanResolve(pi.ParameterType, cm.Constraints[j]))
                    {
                        continue;
                    }

                    // DefaultValue
                    if (pi.HasDefaultValue)
                    {
                        continue;
                    }

                    match = false;
                    break;
                }

                if (match)
                {
                    return cm;
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
        /// <param name="constructor"></param>
        /// <returns></returns>
        private IObjectFactory[] ResolveArgumentsFactories(IKernel kernel, IBinding binding, ConstructorMetadata constructor)
        {
            var parameters = constructor.Parameters;
            var argumentFactories = new IObjectFactory[parameters.Length];

            for (var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];
                var pi = parameter.Parameter;

                // Constructor argument
                var argument = binding.ConstructorArguments.GetParameter(pi.Name);
                if (argument != null)
                {
                    argumentFactories[i] = new ParameterObjectFactory(kernel, argument);
                    continue;
                }

                // Multiple
                if (parameter.ElementType != null)
                {
                    argumentFactories[i] = new ArrayObjectFactory(
                        arrayOperatorFactory.CreateArrayOperator(parameter.ElementType),
                        kernel.ResolveAll(parameter.ElementType, constructor.Constraints[i]).ToArray());
                    continue;
                }

                // Resolve
                var objectFactory = kernel.TryResolve(pi.ParameterType, constructor.Constraints[i], out var resolve);
                if (resolve)
                {
                    argumentFactories[i] = objectFactory;
                    continue;
                }

                // DefaultValue
                if (pi.HasDefaultValue)
                {
                    argumentFactories[i] = new ConstantObjectFactory(pi.DefaultValue);
                }
            }

            return argumentFactories;
        }
    }
}
