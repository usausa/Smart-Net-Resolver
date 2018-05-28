namespace Smart.Resolver.Providers
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    using Smart.ComponentModel;
    using Smart.Reflection;
    using Smart.Resolver.Bindings;
    using Smart.Resolver.Helpers;
    using Smart.Resolver.Injectors;
    using Smart.Resolver.Metadatas;
    using Smart.Resolver.Processors;

    /// <summary>
    ///
    /// </summary>
    public sealed partial class StandardProvider : IProvider
    {
        private readonly IInjector[] injectors;

        private readonly IProcessor[] processors;

        private readonly IDelegateFactory delegateFactory;

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
            return CreateFactory(constructor.Constructor, argumentFactories, processor);
        }

        //private sealed class ConstructorMetadata
        //{
        //    public ConstructorInfo Constructor { get; }

        //    public ParameterMetadata[] Parameters { get; }

        //    public ConstructorMetadata()
        //    {

        //    }
        //}

        //private sealed class ParameterMetadata
        //{
        //    public ParameterInfo Parameter { get; }

        //    public Type ElementType { get; }

        //    public IConstraint Constraint { get; }

        //    public ParameterMetadata(ParameterInfo pi)
        //    {

        //    }
        //}

        // TODO metadata

        private OldConstructorMetadata FindBestConstructor(IKernel kernel, IBinding binding)
        {
            //if (metadata.TargetConstructors.Length == 0)
            //{
            //    throw new InvalidOperationException(
            //        String.Format(CultureInfo.InvariantCulture, "No constructor avaiable. type = {0}", TargetType.Name));
            //}

            //for (var i = 0; i < metadata.TargetConstructors.Length; i++)
            //{
            //    var match = true;
            //    var cm = metadata.TargetConstructors[i];

            //    var parameters = cm.Parameters;
            //    for (var j = 0; j < parameters.Length; j++)
            //    {
            //        var parameter = parameters[j];
            //        var pi = parameter.Parameter;

            //        // Constructor argument
            //        if (binding.ConstructorArguments.GetParameter(pi.Name) != null)
            //        {
            //            continue;
            //        }

            //        // Multiple
            //        if (parameter.ElementType != null)
            //        {
            //            continue;
            //        }

            //        // Resolve
            //        if (kernel.ResolveFactory(pi.ParameterType, cm.Constraints[j]) != null)
            //        {
            //            continue;
            //        }

            //        // DefaultValue
            //        if (pi.HasDefaultValue)
            //        {
            //            continue;
            //        }

            //        match = false;
            //        break;
            //    }

            //    if (match)
            //    {
            //        return cm;
            //    }
            //}

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
        private Func<object>[] ResolveArgumentsFactories(IKernel kernel, IBinding binding, OldConstructorMetadata constructor)
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
                        var arrayFactory = new ArrayFactory(delegateFactory.CreateArrayAllocator(parameter.ElementType), factories);
                        argumentFactories[i] = arrayFactory.Create;
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
            //var targetInjectors = injectors.Where(x => x.IsTarget(kernel, binding, metadata, TargetType)).ToArray();
            //var targetProcessors = processors.Where(x => x.IsTarget(TargetType)).ToArray();

            //if ((targetInjectors.Length > 0) && (targetProcessors.Length > 0))
            //{
            //    return instance =>
            //    {
            //        for (var i = 0; i < targetInjectors.Length; i++)
            //        {
            //            targetInjectors[i].Inject(kernel, binding, metadata, instance);
            //        }

            //        for (var i = 0; i < targetProcessors.Length; i++)
            //        {
            //            targetProcessors[i].Initialize(instance);
            //        }
            //    };
            //}

            //if (targetInjectors.Length > 0)
            //{
            //    return instance =>
            //    {
            //        for (var i = 0; i < targetInjectors.Length; i++)
            //        {
            //            targetInjectors[i].Inject(kernel, binding, metadata, instance);
            //        }
            //    };
            //}

            //if (targetProcessors.Length > 0)
            //{
            //    return instance =>
            //    {
            //        for (var i = 0; i < targetProcessors.Length; i++)
            //        {
            //            targetProcessors[i].Initialize(instance);
            //        }
            //    };
            //}

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ci"></param>
        /// <param name="factories"></param>
        /// <param name="processor"></param>
        /// <returns></returns>
        private Func<object> CreateFactory(ConstructorInfo ci, Func<object>[] factories, Action<object> processor)
        {
            switch (factories.Length)
            {
                case 0:
                    var activator0 = delegateFactory.CreateFactory0(ci);
                    return processor != null ? CreateActivator0(processor, activator0) : activator0;
                case 1:
                    var activator1 = delegateFactory.CreateFactory1(ci);
                    return processor != null ? CreateActivator1(processor, activator1, factories) : CreateActivator1(activator1, factories);
                case 2:
                    var activator2 = delegateFactory.CreateFactory2(ci);
                    return processor != null ? CreateActivator2(processor, activator2, factories) : CreateActivator2(activator2, factories);
                case 3:
                    var activator3 = delegateFactory.CreateFactory3(ci);
                    return processor != null ? CreateActivator3(processor, activator3, factories) : CreateActivator3(activator3, factories);
                case 4:
                    var activator4 = delegateFactory.CreateFactory4(ci);
                    return processor != null ? CreateActivator4(processor, activator4, factories) : CreateActivator4(activator4, factories);
                case 5:
                    var activator5 = delegateFactory.CreateFactory5(ci);
                    return processor != null ? CreateActivator5(processor, activator5, factories) : CreateActivator5(activator5, factories);
                case 6:
                    var activator6 = delegateFactory.CreateFactory6(ci);
                    return processor != null ? CreateActivator6(processor, activator6, factories) : CreateActivator6(activator6, factories);
                case 7:
                    var activator7 = delegateFactory.CreateFactory7(ci);
                    return processor != null ? CreateActivator7(processor, activator7, factories) : CreateActivator7(activator7, factories);
                case 8:
                    var activator8 = delegateFactory.CreateFactory8(ci);
                    return processor != null ? CreateActivator8(processor, activator8, factories) : CreateActivator8(activator8, factories);
                case 9:
                    var activator9 = delegateFactory.CreateFactory9(ci);
                    return processor != null ? CreateActivator9(processor, activator9, factories) : CreateActivator9(activator9, factories);
                case 10:
                    var activator10 = delegateFactory.CreateFactory10(ci);
                    return processor != null ? CreateActivator10(processor, activator10, factories) : CreateActivator10(activator10, factories);
                case 11:
                    var activator11 = delegateFactory.CreateFactory11(ci);
                    return processor != null ? CreateActivator11(processor, activator11, factories) : CreateActivator11(activator11, factories);
                case 12:
                    var activator12 = delegateFactory.CreateFactory12(ci);
                    return processor != null ? CreateActivator12(processor, activator12, factories) : CreateActivator12(activator12, factories);
                case 13:
                    var activator13 = delegateFactory.CreateFactory13(ci);
                    return processor != null ? CreateActivator13(processor, activator13, factories) : CreateActivator13(activator13, factories);
                case 14:
                    var activator14 = delegateFactory.CreateFactory14(ci);
                    return processor != null ? CreateActivator14(processor, activator14, factories) : CreateActivator14(activator14, factories);
                case 15:
                    var activator15 = delegateFactory.CreateFactory15(ci);
                    return processor != null ? CreateActivator15(processor, activator15, factories) : CreateActivator15(activator15, factories);
                case 16:
                    var activator16 = delegateFactory.CreateFactory16(ci);
                    return processor != null ? CreateActivator16(processor, activator16, factories) : CreateActivator16(activator16, factories);
            }

            var activator = delegateFactory.CreateFactory(ci);
            return processor != null ? CreateActivator(processor, activator, factories) : CreateActivator(activator, factories);
        }

        private static Func<object> CreateActivator0(Action<object> processor, Func<object> activator)
        {
            return () =>
            {
                var instance = activator();
                processor(instance);
                return instance;
            };
        }

        private static Func<object> CreateActivator(
            Action<object> processor,
            Func<object[], object> activator,
            Func<object>[] factories)
        {
            return () =>
            {
                var arguments = new object[factories.Length];
                for (var i = 0; i < factories.Length; i++)
                {
                    arguments[i] = factories[i]();
                }

                var instance = activator(arguments);
                processor(instance);
                return instance;
            };
        }

        private static Func<object> CreateActivator(
            Func<object[], object> activator,
            Func<object>[] factories)
        {
            return () =>
            {
                var arguments = new object[factories.Length];
                for (var i = 0; i < factories.Length; i++)
                {
                    arguments[i] = factories[i]();
                }

                return activator(arguments);
            };
        }
    }
}
