namespace Smart.Resolver.Providers
{
    using System;
    using System.Globalization;
    using System.Linq;

    using Smart.ComponentModel;
    using Smart.Reflection;
    using Smart.Resolver.Bindings;
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
        public Func<object> CreateFactory(IKernel kernel, IBinding binding)
        {
            var constructor = FindBestConstructor(kernel, binding);
            var argumentFactories = ResolveArgumentsFactories(kernel, binding, constructor);
            var processor = CreateProcessor(kernel, binding);

            switch (argumentFactories.Length)
            {
                case 0:
                    return FactoryBuilder.Activator0(
                        processor,
                        activatorFactory.CreateActivator<IActivator0>(constructor.Constructor));
                case 1:
                    return FactoryBuilder.Activator1(
                        processor,
                        activatorFactory.CreateActivator<IActivator1>(constructor.Constructor),
                        argumentFactories);
                case 2:
                    return FactoryBuilder.Activator2(
                        processor,
                        activatorFactory.CreateActivator<IActivator2>(constructor.Constructor),
                        argumentFactories);
                case 3:
                    return FactoryBuilder.Activator3(
                        processor,
                        activatorFactory.CreateActivator<IActivator3>(constructor.Constructor),
                        argumentFactories);
                case 4:
                    return FactoryBuilder.Activator4(
                        processor,
                        activatorFactory.CreateActivator<IActivator4>(constructor.Constructor),
                        argumentFactories);
                case 5:
                    return FactoryBuilder.Activator5(
                        processor,
                        activatorFactory.CreateActivator<IActivator5>(constructor.Constructor),
                        argumentFactories);
                case 6:
                    return FactoryBuilder.Activator6(
                        processor,
                        activatorFactory.CreateActivator<IActivator6>(constructor.Constructor),
                        argumentFactories);
                case 7:
                    return FactoryBuilder.Activator7(
                        processor,
                        activatorFactory.CreateActivator<IActivator7>(constructor.Constructor),
                        argumentFactories);
                case 8:
                    return FactoryBuilder.Activator8(
                        processor,
                        activatorFactory.CreateActivator<IActivator8>(constructor.Constructor),
                        argumentFactories);
            }

            return FactoryBuilder.Activator(
                processor,
                activatorFactory.CreateActivator(constructor.Constructor),
                argumentFactories);
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
                    if (kernel.ResolveFactory(pi.ParameterType, cm.Constraints[j]) != null)
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
        private Func<object>[] ResolveArgumentsFactories(IKernel kernel, IBinding binding, ConstructorMetadata constructor)
        {
            var parameters = constructor.Parameters;
            var argumentFactories = new Func<object>[parameters.Length];

            for (var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];
                var pi = parameter.Parameter;

                // Constructor argument
                var argument = binding.ConstructorArguments.GetParameter(pi.Name);
                if (argument != null)
                {
                    argumentFactories[i] = FactoryBuilder.Parameter(kernel, argument);
                    continue;
                }

                // Multiple
                if (parameter.ElementType != null)
                {
                    argumentFactories[i] = FactoryBuilder.Array(
                        arrayOperatorFactory.CreateArrayOperator(parameter.ElementType),
                        kernel.ResolveAllFactory(parameter.ElementType, constructor.Constraints[i]).ToArray());
                    continue;
                }

                // Resolve
                var objectFactory = kernel.ResolveFactory(pi.ParameterType, constructor.Constraints[i]);
                if (objectFactory != null)
                {
                    argumentFactories[i] = objectFactory;
                    continue;
                }

                // DefaultValue
                if (pi.HasDefaultValue)
                {
                    argumentFactories[i] = FactoryBuilder.Constant(pi.DefaultValue);
                }
            }

            return argumentFactories;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="kernel"></param>
        /// <param name="binding"></param>
        /// <returns></returns>
        private Action<object> CreateProcessor(IKernel kernel, IBinding binding)
        {
            var targetInjectors = injectors.Where(x => x.IsTarget(kernel, binding, metadata, TargetType)).ToArray();
            var targetProcessors = processors.Where(x => x.IsTarget(TargetType)).ToArray();

            if ((targetInjectors.Length > 0) && (targetProcessors.Length > 0))
            {
                return instance =>
                {
                    for (var i = 0; i < targetInjectors.Length; i++)
                    {
                        targetInjectors[i].Inject(kernel, binding, metadata, instance);
                    }

                    for (var i = 0; i < targetProcessors.Length; i++)
                    {
                        targetProcessors[i].Initialize(instance);
                    }
                };
            }

            if (targetInjectors.Length > 0)
            {
                return instance =>
                {
                    for (var i = 0; i < targetInjectors.Length; i++)
                    {
                        targetInjectors[i].Inject(kernel, binding, metadata, instance);
                    }
                };
            }

            if (targetProcessors.Length > 0)
            {
                return instance =>
                {
                    for (var i = 0; i < targetProcessors.Length; i++)
                    {
                        targetProcessors[i].Initialize(instance);
                    }
                };
            }

            return null;
        }
    }
}
