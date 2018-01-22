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
            var processor = CreateProcessor(kernel, binding);

            switch (argumentFactories.Length)
            {
                case 0:
                    var activator0 = activatorFactory.CreateActivator<IActivator0>(constructor.Constructor);
                    return processor != null
                        ? (IObjectFactory)new Activator0ObjectFactory(processor, activator0)
                        : new NoProcessActivator0ObjectFactory(activator0);
                case 1:
                    var activator1 = activatorFactory.CreateActivator<IActivator1>(constructor.Constructor);
                    return processor != null
                        ? (IObjectFactory)new Activator1ObjectFactory(
                            processor,
                            activator1,
                            argumentFactories[0])
                        : new NoProcessActivator1ObjectFactory(
                            activator1,
                            argumentFactories[0]);
                case 2:
                    var activator2 = activatorFactory.CreateActivator<IActivator2>(constructor.Constructor);
                    return processor != null
                        ? (IObjectFactory)new Activator2ObjectFactory(
                            processor,
                            activator2,
                            argumentFactories[0],
                            argumentFactories[1])
                        : new NoProcessActivator2ObjectFactory(
                            activator2,
                            argumentFactories[0],
                            argumentFactories[1]);
                case 3:
                    var activator3 = activatorFactory.CreateActivator<IActivator3>(constructor.Constructor);
                    return processor != null
                        ? (IObjectFactory)new Activator3ObjectFactory(
                            processor,
                            activator3,
                            argumentFactories[0],
                            argumentFactories[1],
                            argumentFactories[2])
                        : new NoProcessActivator3ObjectFactory(
                            activator3,
                            argumentFactories[0],
                            argumentFactories[1],
                            argumentFactories[2]);
                case 4:
                    var activator4 = activatorFactory.CreateActivator<IActivator4>(constructor.Constructor);
                    return processor != null
                        ? (IObjectFactory)new Activator4ObjectFactory(
                            processor,
                            activator4,
                            argumentFactories[0],
                            argumentFactories[1],
                            argumentFactories[2],
                            argumentFactories[3])
                        : new NoProcessActivator4ObjectFactory(
                            activator4,
                            argumentFactories[0],
                            argumentFactories[1],
                            argumentFactories[2],
                            argumentFactories[3]);
                case 5:
                    var activator5 = activatorFactory.CreateActivator<IActivator5>(constructor.Constructor);
                    return processor != null
                        ? (IObjectFactory)new Activator5ObjectFactory(
                            processor,
                            activator5,
                            argumentFactories[0],
                            argumentFactories[1],
                            argumentFactories[2],
                            argumentFactories[3],
                            argumentFactories[4])
                        : new NoProcessActivator5ObjectFactory(
                            activator5,
                            argumentFactories[0],
                            argumentFactories[1],
                            argumentFactories[2],
                            argumentFactories[3],
                            argumentFactories[4]);
                case 6:
                    var activator6 = activatorFactory.CreateActivator<IActivator6>(constructor.Constructor);
                    return processor != null
                        ? (IObjectFactory)new Activator6ObjectFactory(
                            processor,
                            activator6,
                            argumentFactories[0],
                            argumentFactories[1],
                            argumentFactories[2],
                            argumentFactories[3],
                            argumentFactories[4],
                            argumentFactories[5])
                        : new NoProcessActivator6ObjectFactory(
                            activator6,
                            argumentFactories[0],
                            argumentFactories[1],
                            argumentFactories[2],
                            argumentFactories[3],
                            argumentFactories[4],
                            argumentFactories[5]);
                case 7:
                    var activator7 = activatorFactory.CreateActivator<IActivator7>(constructor.Constructor);
                    return processor != null
                        ? (IObjectFactory)new Activator7ObjectFactory(
                            processor,
                            activator7,
                            argumentFactories[0],
                            argumentFactories[1],
                            argumentFactories[2],
                            argumentFactories[3],
                            argumentFactories[4],
                            argumentFactories[5],
                            argumentFactories[6])
                        : new NoProcessActivator7ObjectFactory(
                            activator7,
                            argumentFactories[0],
                            argumentFactories[1],
                            argumentFactories[2],
                            argumentFactories[3],
                            argumentFactories[4],
                            argumentFactories[5],
                            argumentFactories[6]);
                case 8:
                    var activator8 = activatorFactory.CreateActivator<IActivator8>(constructor.Constructor);
                    return processor != null
                        ? (IObjectFactory)new Activator8ObjectFactory(
                            processor,
                            activator8,
                            argumentFactories[0],
                            argumentFactories[1],
                            argumentFactories[2],
                            argumentFactories[3],
                            argumentFactories[4],
                            argumentFactories[5],
                            argumentFactories[6],
                            argumentFactories[7])
                        : new NoProcessActivator8ObjectFactory(
                            activator8,
                            argumentFactories[0],
                            argumentFactories[1],
                            argumentFactories[2],
                            argumentFactories[3],
                            argumentFactories[4],
                            argumentFactories[5],
                            argumentFactories[6],
                            argumentFactories[7]);
            }

            var activator = activatorFactory.CreateActivator(constructor.Constructor);
            return processor != null
                ? (IObjectFactory)new ActivatorObjectFactory(processor, activator, argumentFactories)
                : new NoProcessActivatorObjectFactory(activator, argumentFactories);
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
                    argumentFactories[i] = new ConstantObjectFactory(pi.DefaultValue);
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
