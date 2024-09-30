namespace Smart.Resolver.Providers;

using System.Globalization;
using System.Reflection;

using Smart.ComponentModel;
using Smart.Resolver.Attributes;
using Smart.Resolver.Bindings;
using Smart.Resolver.Builders;
using Smart.Resolver.Helpers;
using Smart.Resolver.Injectors;
using Smart.Resolver.Keys;
using Smart.Resolver.Processors;

public sealed class StandardProvider : IProvider
{
    private readonly IInjector[] injectors;

    private readonly IProcessor[] processors;

    private readonly IKeySource[] keySources;

    private readonly IFactoryBuilder builder;

    public Type TargetType { get; }

    public StandardProvider(Type type, ComponentContainer components)
    {
        TargetType = type;
        injectors = components.GetAll<IInjector>().ToArray();
        processors = components.GetAll<IProcessor>().ToArray();
        keySources = components.GetAll<IKeySource>().ToArray();
        builder = components.Get<IFactoryBuilder>();
    }

    public Func<IResolver, object> CreateFactory(IKernel kernel, Binding binding)
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
            var argumentFactories = new List<Func<IResolver, object?>>(constructor.Parameters.Length);

            foreach (var parameter in constructor.Parameters)
            {
                var pi = parameter.Parameter;

                // Constructor argument
                var argument = binding.ConstructorArguments.GetParameter(pi.Name!);
                if (argument is not null)
                {
                    argumentFactories.Add(argument.Resolve);
                    continue;
                }

                // Resolve
                if (kernel.TryResolveFactory(pi.ParameterType, parameter.ResolveBy, out var factory))
                {
                    argumentFactories.Add(factory);
                    continue;
                }

                // Multiple
                if ((parameter.ElementType is not null) &&
                    kernel.TryResolveFactories(parameter.ElementType, parameter.ResolveBy, out var factories))
                {
                    var arrayFactory = builder.CreateArrayFactory(parameter.ElementType, factories);
                    argumentFactories.Add(arrayFactory);
                    continue;
                }

                // DefaultValue
                if (pi.HasDefaultValue)
                {
                    argumentFactories.Add(_ => pi.DefaultValue);
                    continue;
                }

                match = false;
                break;
            }

            if (match)
            {
                var actions = CreateActions(binding);
                return builder.CreateFactory(constructor.Constructor, [.. argumentFactories], actions);
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
            .Where(static c => !c.IsStatic)
            .OrderByDescending(static c => c.IsInjectDefined() ? 1 : 0)
            .ThenByDescending(static c => c.GetParameters().Length)
            .ThenByDescending(static c => c.GetParameters().Count(static p => p.HasDefaultValue))
            .Select(c => new ConstructorMetadata(
                c,
                c.GetParameters()
                    .Select(p => new ParameterMetadata(p, KeySourceHelper.GetValue(p, keySources)))
                    .ToArray()))
            .ToArray();
    }

    private Action<IResolver, object>[] CreateActions(Binding binding)
    {
        var targetInjectors = injectors
            .Select(x => x.CreateInjector(TargetType, binding));
        var targetProcessors = processors
            .OrderByDescending(static x => x.Order)
            .Select(x => x.CreateProcessor(TargetType));
        return targetInjectors.Concat(targetProcessors).Where(static x => x is not null).ToArray()!;
    }

    // ------------------------------------------------------------
    // Metadata
    // ------------------------------------------------------------

    private sealed class ConstructorMetadata
    {
        public ConstructorInfo Constructor { get; }

        public ParameterMetadata[] Parameters { get; }

        public ConstructorMetadata(ConstructorInfo ci, ParameterMetadata[] parameters)
        {
            Constructor = ci;
            Parameters = parameters;
        }
    }

    private sealed class ParameterMetadata
    {
        public ParameterInfo Parameter { get; }

        public Type? ElementType { get; }

        public object? ResolveBy { get; }

        public ParameterMetadata(ParameterInfo pi, object? resolveBy)
        {
            Parameter = pi;
            ElementType = TypeHelper.GetEnumerableElementType(pi.ParameterType);
            ResolveBy = resolveBy;
        }
    }
}
