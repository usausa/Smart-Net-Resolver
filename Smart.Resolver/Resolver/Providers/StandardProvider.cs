namespace Smart.Resolver.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    using Smart.ComponentModel;
    using Smart.Reflection;
    using Smart.Resolver.Attributes;
    using Smart.Resolver.Bindings;
    using Smart.Resolver.Constraints;
    using Smart.Resolver.Helpers;
    using Smart.Resolver.Injectors;
    using Smart.Resolver.Processors;

    public sealed partial class StandardProvider : IProvider
    {
        private readonly IInjector[] injectors;

        private readonly IProcessor[] processors;

        private readonly IDelegateFactory delegateFactory;

        public Type TargetType { get; }

        public StandardProvider(Type type, IComponentContainer components)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (components is null)
            {
                throw new ArgumentNullException(nameof(components));
            }

            TargetType = type;
            injectors = components.GetAll<IInjector>().ToArray();
            processors = components.GetAll<IProcessor>().ToArray();
            delegateFactory = components.Get<IDelegateFactory>();
        }

        public Func<IResolver, object> CreateFactory(IKernel kernel, IBinding binding)
        {
            var constructors = CreateConstructorMetadata();
            if (constructors.Length == 0)
            {
                throw new InvalidOperationException(
                    String.Format(CultureInfo.InvariantCulture, "No constructor available. type = {0}", TargetType.Name));
            }

            foreach (var constructor in constructors)
            {
                var match = true;
                var argumentFactories = new List<Func<IResolver, object>>(constructor.Parameters.Length);

                foreach (var parameter in constructor.Parameters)
                {
                    var pi = parameter.Parameter;

                    // Constructor argument
                    var argument = binding.ConstructorArguments.GetParameter(pi.Name);
                    if (argument != null)
                    {
                        argumentFactories.Add(k => argument.Resolve(k));
                        continue;
                    }

                    // Resolve
                    if (kernel.TryResolveFactory(pi.ParameterType, parameter.Constraint, out var factory))
                    {
                        argumentFactories.Add(factory);
                        continue;
                    }

                    // Multiple
                    if ((parameter.ElementType != null) &&
                        kernel.TryResolveFactories(parameter.ElementType, parameter.Constraint, out var factories))
                    {
                        var arrayFactory = ArrayFactory.Create(delegateFactory.CreateArrayAllocator(parameter.ElementType), factories);
                        argumentFactories.Add(arrayFactory);
                        continue;
                    }

                    // DefaultValue
                    if (pi.HasDefaultValue)
                    {
                        argumentFactories.Add(k => pi.DefaultValue);
                        continue;
                    }

                    match = false;
                    break;
                }

                if (match)
                {
                    var actions = CreateActions(binding);
                    return CreateFactory(constructor.Constructor, argumentFactories.ToArray(), actions);
                }
            }

            throw new InvalidOperationException(
            String.Format(CultureInfo.InvariantCulture, "Constructor parameter unresolved. type = {0}", TargetType.Name));
        }

        // ------------------------------------------------------------
        // Helpers
        // ------------------------------------------------------------

        private ConstructorMetadata[] CreateConstructorMetadata()
        {
            return TargetType.GetConstructors()
                .Where(c => !c.IsStatic)
                .OrderByDescending(c => c.IsInjectDefined() ? 1 : 0)
                .ThenByDescending(c => c.GetParameters().Length)
                .ThenByDescending(c => c.GetParameters().Count(p => p.HasDefaultValue))
                .Select(c => new ConstructorMetadata(c))
                .ToArray();
        }

        private Action<IResolver, object>[] CreateActions(IBinding binding)
        {
            var targetInjectors = injectors
                .Select(x => x.CreateInjector(TargetType, binding))
                .Where(x => x != null);
            var targetProcessors = processors
                .OrderByDescending(x => x.Order)
                .Select(x => x.CreateProcessor(TargetType))
                .Where(x => x != null);
            return targetInjectors.Concat(targetProcessors).ToArray();
        }

        private Func<IResolver, object> CreateFactory(ConstructorInfo ci, Func<IResolver, object>[] factories, Action<IResolver, object>[] actions)
        {
            switch (factories.Length)
            {
                case 0:
                    var activator0 = delegateFactory.CreateFactory0(ci);
                    return actions.Length > 0 ? CreateActivator0(activator0, actions) : k => activator0();
                case 1:
                    var activator1 = delegateFactory.CreateFactory1(ci);
                    return actions.Length > 0 ? CreateActivator1(activator1, factories, actions) : CreateActivator1(activator1, factories);
                case 2:
                    var activator2 = delegateFactory.CreateFactory2(ci);
                    return actions.Length > 0 ? CreateActivator2(activator2, factories, actions) : CreateActivator2(activator2, factories);
                case 3:
                    var activator3 = delegateFactory.CreateFactory3(ci);
                    return actions.Length > 0 ? CreateActivator3(activator3, factories, actions) : CreateActivator3(activator3, factories);
                case 4:
                    var activator4 = delegateFactory.CreateFactory4(ci);
                    return actions.Length > 0 ? CreateActivator4(activator4, factories, actions) : CreateActivator4(activator4, factories);
                case 5:
                    var activator5 = delegateFactory.CreateFactory5(ci);
                    return actions.Length > 0 ? CreateActivator5(activator5, factories, actions) : CreateActivator5(activator5, factories);
                case 6:
                    var activator6 = delegateFactory.CreateFactory6(ci);
                    return actions.Length > 0 ? CreateActivator6(activator6, factories, actions) : CreateActivator6(activator6, factories);
                case 7:
                    var activator7 = delegateFactory.CreateFactory7(ci);
                    return actions.Length > 0 ? CreateActivator7(activator7, factories, actions) : CreateActivator7(activator7, factories);
                case 8:
                    var activator8 = delegateFactory.CreateFactory8(ci);
                    return actions.Length > 0 ? CreateActivator8(activator8, factories, actions) : CreateActivator8(activator8, factories);
                case 9:
                    var activator9 = delegateFactory.CreateFactory9(ci);
                    return actions.Length > 0 ? CreateActivator9(activator9, factories, actions) : CreateActivator9(activator9, factories);
                case 10:
                    var activator10 = delegateFactory.CreateFactory10(ci);
                    return actions.Length > 0 ? CreateActivator10(activator10, factories, actions) : CreateActivator10(activator10, factories);
                case 11:
                    var activator11 = delegateFactory.CreateFactory11(ci);
                    return actions.Length > 0 ? CreateActivator11(activator11, factories, actions) : CreateActivator11(activator11, factories);
                case 12:
                    var activator12 = delegateFactory.CreateFactory12(ci);
                    return actions.Length > 0 ? CreateActivator12(activator12, factories, actions) : CreateActivator12(activator12, factories);
                case 13:
                    var activator13 = delegateFactory.CreateFactory13(ci);
                    return actions.Length > 0 ? CreateActivator13(activator13, factories, actions) : CreateActivator13(activator13, factories);
                case 14:
                    var activator14 = delegateFactory.CreateFactory14(ci);
                    return actions.Length > 0 ? CreateActivator14(activator14, factories, actions) : CreateActivator14(activator14, factories);
                case 15:
                    var activator15 = delegateFactory.CreateFactory15(ci);
                    return actions.Length > 0 ? CreateActivator15(activator15, factories, actions) : CreateActivator15(activator15, factories);
                case 16:
                    var activator16 = delegateFactory.CreateFactory16(ci);
                    return actions.Length > 0 ? CreateActivator16(activator16, factories, actions) : CreateActivator16(activator16, factories);
            }

            var activator = delegateFactory.CreateFactory(ci);
            return actions.Length > 0 ? CreateActivator(activator, factories, actions) : CreateActivator(activator, factories);
        }

        // ------------------------------------------------------------
        // Activator
        // ------------------------------------------------------------

        private static Func<IResolver, object> CreateActivator0(
            Func<object> activator,
            Action<IResolver, object>[] actions)
        {
            return k =>
            {
                var instance = activator();

                for (var i = 0; i < actions.Length; i++)
                {
                    actions[i](k, instance);
                }

                return instance;
            };
        }

        private static Func<IResolver, object> CreateActivator(
            Func<object[], object> activator,
            Func<IResolver, object>[] factories,
            Action<IResolver, object>[] actions)
        {
            return k =>
            {
                var arguments = new object[factories.Length];
                for (var i = 0; i < factories.Length; i++)
                {
                    arguments[i] = factories[i](k);
                }

                var instance = activator(arguments);

                for (var i = 0; i < actions.Length; i++)
                {
                    actions[i](k, instance);
                }

                return instance;
            };
        }

        private static Func<IResolver, object> CreateActivator(
            Func<object[], object> activator,
            Func<IResolver, object>[] factories)
        {
            return k =>
            {
                var arguments = new object[factories.Length];
                for (var i = 0; i < factories.Length; i++)
                {
                    arguments[i] = factories[i](k);
                }

                return activator(arguments);
            };
        }

        // ------------------------------------------------------------
        // Metadata
        // ------------------------------------------------------------

        private sealed class ConstructorMetadata
        {
            public ConstructorInfo Constructor { get; }

            public ParameterMetadata[] Parameters { get; }

            public ConstructorMetadata(ConstructorInfo ci)
            {
                Constructor = ci;
                Parameters = ci.GetParameters()
                    .Select(x => new ParameterMetadata(x))
                    .ToArray();
            }
        }

        private sealed class ParameterMetadata
        {
            public ParameterInfo Parameter { get; }

            public Type ElementType { get; }

            public IConstraint Constraint { get; }

            public ParameterMetadata(ParameterInfo pi)
            {
                Parameter = pi;
                ElementType = TypeHelper.GetEnumerableElementType(pi.ParameterType);
                Constraint = ConstraintBuilder.Build(pi.GetCustomAttributes<ConstraintAttribute>());
            }
        }
    }
}
