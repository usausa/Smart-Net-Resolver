namespace Smart.Resolver.Injectors;

using System.Reflection;
using System.Runtime.CompilerServices;

using Smart.Reflection;
using Smart.Resolver.Attributes;
using Smart.Resolver.Bindings;
using Smart.Resolver.Parameters;

public sealed class PropertyInjector : IInjector
{
    private readonly IDelegateFactory delegateFactory;

    public PropertyInjector(IDelegateFactory delegateFactory)
    {
        this.delegateFactory = delegateFactory;
    }

    public Action<IResolver, object>? CreateInjector(Type type, Binding binding)
    {
        var entries = type.GetRuntimeProperties()
            .Where(static p => p.IsInjectDefined())
            .Select(x => CreateInjectEntry(x, binding))
            .ToArray();
        if (entries.Length == 0)
        {
            return null;
        }

        var injector = new Injector(entries);
        return injector.Inject;
    }

    private InjectEntry CreateInjectEntry(PropertyInfo pi, Binding binding)
    {
        var setter = delegateFactory.CreateSetter(pi, true)!;

        var parameter = binding.PropertyValues.GetParameter(pi.Name);
        if (parameter is not null)
        {
            return new InjectEntry(CreateParameterProvider(parameter), setter);
        }

        // TODO key and compatibility
        var propertyType = delegateFactory.GetExtendedPropertyType(pi);
        var keyed = pi.GetCustomAttribute<KeyedAttribute>();
        if (keyed is not null)
        {
            return new InjectEntry(CreateConstraintProvider(propertyType, keyed), setter);
        }

        return new InjectEntry(CreateProvider(propertyType), setter);
    }

    private static Func<IResolver, object?> CreateParameterProvider(IParameter parameter)
    {
        return parameter.Resolve;
    }

    private static Func<IResolver, object> CreateConstraintProvider(Type propertyType, object? parameter)
    {
        return resolver => resolver.Get(propertyType, parameter);
    }

    private static Func<IResolver, object> CreateProvider(Type propertyType)
    {
        return resolver => resolver.Get(propertyType);
    }

    private sealed class InjectEntry
    {
        private readonly Func<IResolver, object?> provider;

        private readonly Action<object, object?> setter;

        public InjectEntry(Func<IResolver, object?> provider, Action<object, object?> setter)
        {
            this.provider = provider;
            this.setter = setter;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Inject(IResolver resolver, object instance)
        {
            setter(instance, provider(resolver));
        }
    }

    private sealed class Injector
    {
        private readonly InjectEntry[] entries;

        public Injector(InjectEntry[] entries)
        {
            this.entries = entries;
        }

        public void Inject(IResolver resolver, object instance)
        {
            var entriesLocal = entries;
            for (var i = 0; i < entriesLocal.Length; i++)
            {
                entriesLocal[i].Inject(resolver, instance);
            }
        }
    }
}
