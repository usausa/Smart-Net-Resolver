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

        private readonly IDelegateFactory delegateFactory;

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
            delegateFactory = components.Get<IDelegateFactory>();
            metadata = components.Get<IMetadataFactory>().GetMetadata(TargetType);
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
                    return Activator0(
                        processor,
                        delegateFactory.CreateFactory0(constructor.Constructor));
                case 1:
                    return Activator1(
                        processor,
                        delegateFactory.CreateFactory1(constructor.Constructor),
                        argumentFactories);
                case 2:
                    return Activator2(
                        processor,
                        delegateFactory.CreateFactory2(constructor.Constructor),
                        argumentFactories);
                case 3:
                    return Activator3(
                        processor,
                        delegateFactory.CreateFactory3(constructor.Constructor),
                        argumentFactories);
                case 4:
                    return Activator4(
                        processor,
                        delegateFactory.CreateFactory4(constructor.Constructor),
                        argumentFactories);
                case 5:
                    return Activator5(
                        processor,
                        delegateFactory.CreateFactory5(constructor.Constructor),
                        argumentFactories);
                case 6:
                    return Activator6(
                        processor,
                        delegateFactory.CreateFactory6(constructor.Constructor),
                        argumentFactories);
                case 7:
                    return Activator7(
                        processor,
                        delegateFactory.CreateFactory7(constructor.Constructor),
                        argumentFactories);
                case 8:
                    return Activator8(
                        processor,
                        delegateFactory.CreateFactory8(constructor.Constructor),
                        argumentFactories);
            }

            return Activator(
                processor,
                delegateFactory.CreateFactory(constructor.Constructor),
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
                    argumentFactories[i] = () => argument.Resolve(kernel);
                    continue;
                }

                // Multiple
                if (parameter.ElementType != null)
                {
                    argumentFactories[i] = kernel.ResolveFactory(pi.ParameterType, constructor.Constraints[i]);
                    if (argumentFactories[i] == null)
                    {
                        var factories = kernel.ResolveAllFactory(parameter.ElementType, constructor.Constraints[i]).ToArray();
                        var array = delegateFactory.CreateArrayAllocator(parameter.ElementType)(factories.Length);
                        var objs = (object[])array;
                        for (var j = 0; j < factories.Length; j++)
                        {
                            objs[j] = factories[j]();
                        }

                        argumentFactories[i] = () => array;
                    }

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
                    argumentFactories[i] = () => pi.DefaultValue;
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

        // TODO

        private static Func<object> Activator0(
            Action<object> processor,
            Func<object> activator)
        {
            if (processor != null)
            {
                return () =>
                {
                    var instance = activator();
                    processor(instance);
                    return instance;
                };
            }

            return activator;
        }

        private static Func<object> Activator1(
            Action<object> processor,
            Func<object, object> activator,
            Func<object>[] argumentFactories)
        {
            if (processor != null)
            {
                return Activator1WithProcess(
                    processor,
                    activator,
                    argumentFactories[0]);
            }

            return Activator1WithoutProcess(
                activator,
                argumentFactories[0]);
        }

        private static Func<object> Activator1WithProcess(
            Action<object> processor,
            Func<object, object> activator,
            Func<object> f1)
        {
            return () =>
            {
                var instance = activator(f1());
                processor(instance);
                return instance;
            };
        }

        private static Func<object> Activator1WithoutProcess(
            Func<object, object> activator,
            Func<object> f1)
        {
            return () => activator(f1());
        }

        private static Func<object> Activator2(
            Action<object> processor,
            Func<object, object, object> activator,
            Func<object>[] argumentFactories)
        {
            if (processor != null)
            {
                return Activator2WithProcess(
                    processor,
                    activator,
                    argumentFactories[0],
                    argumentFactories[1]);
            }

            return Activator2WithoutProcess(
                activator,
                argumentFactories[0],
                argumentFactories[1]);
        }

        private static Func<object> Activator2WithProcess(
            Action<object> processor,
            Func<object, object, object> activator,
            Func<object> f1,
            Func<object> f2)
        {
            return () =>
            {
                var instance = activator(f1(), f2());
                processor(instance);
                return instance;
            };
        }

        private static Func<object> Activator2WithoutProcess(
            Func<object, object, object> activator,
            Func<object> f1,
            Func<object> f2)
        {
            return () => activator(f1(), f2());
        }

        private static Func<object> Activator3(
            Action<object> processor,
            Func<object, object, object, object> activator,
            Func<object>[] argumentFactories)
        {
            if (processor != null)
            {
                return Activator3WithProcess(
                    processor,
                    activator,
                    argumentFactories[0],
                    argumentFactories[1],
                    argumentFactories[2]);
            }

            return Activator3WithoutProcess(
                activator,
                argumentFactories[0],
                argumentFactories[1],
                argumentFactories[2]);
        }

        private static Func<object> Activator3WithProcess(
            Action<object> processor,
            Func<object, object, object, object> activator,
            Func<object> f1,
            Func<object> f2,
            Func<object> f3)
        {
            return () =>
            {
                var instance = activator(f1(), f2(), f3());
                processor(instance);
                return instance;
            };
        }

        private static Func<object> Activator3WithoutProcess(
            Func<object, object, object, object> activator,
            Func<object> f1,
            Func<object> f2,
            Func<object> f3)
        {
            return () => activator(f1(), f2(), f3());
        }

        private static Func<object> Activator4(
            Action<object> processor,
            Func<object, object, object, object, object> activator,
            Func<object>[] argumentFactories)
        {
            if (processor != null)
            {
                return Activator4WithProcess(
                    processor,
                    activator,
                    argumentFactories[0],
                    argumentFactories[1],
                    argumentFactories[2],
                    argumentFactories[3]);
            }

            return Activator4WithoutProcess(
                activator,
                argumentFactories[0],
                argumentFactories[1],
                argumentFactories[2],
                argumentFactories[3]);
        }

        private static Func<object> Activator4WithProcess(
            Action<object> processor,
            Func<object, object, object, object, object> activator,
            Func<object> f1,
            Func<object> f2,
            Func<object> f3,
            Func<object> f4)
        {
            return () =>
            {
                var instance = activator(f1(), f2(), f3(), f4());
                processor(instance);
                return instance;
            };
        }

        private static Func<object> Activator4WithoutProcess(
            Func<object, object, object, object, object> activator,
            Func<object> f1,
            Func<object> f2,
            Func<object> f3,
            Func<object> f4)
        {
            return () => activator(f1(), f2(), f3(), f4());
        }

        private static Func<object> Activator5(
            Action<object> processor,
            Func<object, object, object, object, object, object> activator,
            Func<object>[] argumentFactories)
        {
            if (processor != null)
            {
                return Activator5WithProcess(
                    processor,
                    activator,
                    argumentFactories[0],
                    argumentFactories[1],
                    argumentFactories[2],
                    argumentFactories[3],
                    argumentFactories[4]);
            }

            return Activator5WithoutProcess(
                activator,
                argumentFactories[0],
                argumentFactories[1],
                argumentFactories[2],
                argumentFactories[3],
                argumentFactories[4]);
        }

        private static Func<object> Activator5WithProcess(
            Action<object> processor,
            Func<object, object, object, object, object, object> activator,
            Func<object> f1,
            Func<object> f2,
            Func<object> f3,
            Func<object> f4,
            Func<object> f5)
        {
            return () =>
            {
                var instance = activator(f1(), f2(), f3(), f4(), f5());
                processor(instance);
                return instance;
            };
        }

        private static Func<object> Activator5WithoutProcess(
            Func<object, object, object, object, object, object> activator,
            Func<object> f1,
            Func<object> f2,
            Func<object> f3,
            Func<object> f4,
            Func<object> f5)
        {
            return () => activator(f1(), f2(), f3(), f4(), f5());
        }

        private static Func<object> Activator6(
            Action<object> processor,
            Func<object, object, object, object, object, object, object> activator,
            Func<object>[] argumentFactories)
        {
            if (processor != null)
            {
                return Activator6WithProcess(
                    processor,
                    activator,
                    argumentFactories[0],
                    argumentFactories[1],
                    argumentFactories[2],
                    argumentFactories[3],
                    argumentFactories[4],
                    argumentFactories[5]);
            }

            return Activator6WithoutProcess(
                activator,
                argumentFactories[0],
                argumentFactories[1],
                argumentFactories[2],
                argumentFactories[3],
                argumentFactories[4],
                argumentFactories[5]);
        }

        private static Func<object> Activator6WithProcess(
            Action<object> processor,
            Func<object, object, object, object, object, object, object> activator,
            Func<object> f1,
            Func<object> f2,
            Func<object> f3,
            Func<object> f4,
            Func<object> f5,
            Func<object> f6)
        {
            return () =>
            {
                var instance = activator(f1(), f2(), f3(), f4(), f5(), f6());
                processor(instance);
                return instance;
            };
        }

        private static Func<object> Activator6WithoutProcess(
            Func<object, object, object, object, object, object, object> activator,
            Func<object> f1,
            Func<object> f2,
            Func<object> f3,
            Func<object> f4,
            Func<object> f5,
            Func<object> f6)
        {
            return () => activator(f1(), f2(), f3(), f4(), f5(), f6());
        }

        private static Func<object> Activator7(
            Action<object> processor,
            Func<object, object, object, object, object, object, object, object> activator,
            Func<object>[] argumentFactories)
        {
            if (processor != null)
            {
                return Activator7WithProcess(
                    processor,
                    activator,
                    argumentFactories[0],
                    argumentFactories[1],
                    argumentFactories[2],
                    argumentFactories[3],
                    argumentFactories[4],
                    argumentFactories[5],
                    argumentFactories[6]);
            }

            return Activator7WithoutProcess(
                activator,
                argumentFactories[0],
                argumentFactories[1],
                argumentFactories[2],
                argumentFactories[3],
                argumentFactories[4],
                argumentFactories[5],
                argumentFactories[6]);
        }

        private static Func<object> Activator7WithProcess(
            Action<object> processor,
            Func<object, object, object, object, object, object, object, object> activator,
            Func<object> f1,
            Func<object> f2,
            Func<object> f3,
            Func<object> f4,
            Func<object> f5,
            Func<object> f6,
            Func<object> f7)
        {
            return () =>
            {
                var instance = activator(f1(), f2(), f3(), f4(), f5(), f6(), f7());
                processor(instance);
                return instance;
            };
        }

        private static Func<object> Activator7WithoutProcess(
            Func<object, object, object, object, object, object, object, object> activator,
            Func<object> f1,
            Func<object> f2,
            Func<object> f3,
            Func<object> f4,
            Func<object> f5,
            Func<object> f6,
            Func<object> f7)
        {
            return () => activator(f1(), f2(), f3(), f4(), f5(), f6(), f7());
        }

        private static Func<object> Activator8(
            Action<object> processor,
            Func<object, object, object, object, object, object, object, object, object> activator,
            Func<object>[] argumentFactories)
        {
            if (processor != null)
            {
                return Activator8WithProcess(
                    processor,
                    activator,
                    argumentFactories[0],
                    argumentFactories[1],
                    argumentFactories[2],
                    argumentFactories[3],
                    argumentFactories[4],
                    argumentFactories[5],
                    argumentFactories[6],
                    argumentFactories[7]);
            }

            return Activator8WithoutProcess(
                activator,
                argumentFactories[0],
                argumentFactories[1],
                argumentFactories[2],
                argumentFactories[3],
                argumentFactories[4],
                argumentFactories[5],
                argumentFactories[6],
                argumentFactories[7]);
        }

        private static Func<object> Activator8WithProcess(
            Action<object> processor,
            Func<object, object, object, object, object, object, object, object, object> activator,
            Func<object> f1,
            Func<object> f2,
            Func<object> f3,
            Func<object> f4,
            Func<object> f5,
            Func<object> f6,
            Func<object> f7,
            Func<object> f8)
        {
            return () =>
            {
                var instance = activator(f1(), f2(), f3(), f4(), f5(), f6(), f7(), f8());
                processor(instance);
                return instance;
            };
        }

        private static Func<object> Activator8WithoutProcess(
            Func<object, object, object, object, object, object, object, object, object> activator,
            Func<object> f1,
            Func<object> f2,
            Func<object> f3,
            Func<object> f4,
            Func<object> f5,
            Func<object> f6,
            Func<object> f7,
            Func<object> f8)
        {
            return () => activator(f1(), f2(), f3(), f4(), f5(), f6(), f7(), f8());
        }

        private static Func<object> Activator(
            Action<object> processor,
            Func<object[], object> activator,
            Func<object>[] argumentFactories)
        {
            return processor != null
                ? ActivatorWithProcess(processor, activator, argumentFactories)
                : ActivatorWithoutProcess(activator, argumentFactories);
        }

        private static Func<object> ActivatorWithProcess(
            Action<object> processor,
            Func<object[], object> activator,
            Func<object>[] argumentFactories)
        {
            return () =>
            {
                var arguments = new object[argumentFactories.Length];
                for (var i = 0; i < argumentFactories.Length; i++)
                {
                    arguments[i] = argumentFactories[i]();
                }

                var instance = activator(arguments);
                processor(instance);
                return instance;
            };
        }

        private static Func<object> ActivatorWithoutProcess(
            Func<object[], object> activator,
            Func<object>[] argumentFactories)
        {
            return () =>
            {
                var arguments = new object[argumentFactories.Length];
                for (var i = 0; i < argumentFactories.Length; i++)
                {
                    arguments[i] = argumentFactories[i]();
                }

                return activator(arguments);
            };
        }
    }
}
