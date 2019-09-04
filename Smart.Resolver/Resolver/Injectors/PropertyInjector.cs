namespace Smart.Resolver.Injectors
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Smart.Reflection;
    using Smart.Resolver.Attributes;
    using Smart.Resolver.Bindings;
    using Smart.Resolver.Constraints;
    using Smart.Resolver.Parameters;

    public sealed class PropertyInjector : IInjector
    {
        private readonly IDelegateFactory delegateFactory;

        public PropertyInjector(IDelegateFactory delegateFactory)
        {
            this.delegateFactory = delegateFactory;
        }

        public Action<IResolver, object> CreateInjector(Type type, IBinding binding)
        {
            var entries = type.GetRuntimeProperties()
                .Where(p => p.IsInjectDefined())
                .Select(x => CreateInjectEntry(x, binding))
                .ToArray();
            if (entries.Length == 0)
            {
                return null;
            }

            var injector = new Injector { Entries = entries };
            return injector.Inject;
        }

        private InjectEntry CreateInjectEntry(PropertyInfo pi, IBinding binding)
        {
            var setter = delegateFactory.CreateSetter(pi, true);

            var parameter = binding.PropertyValues.GetParameter(pi.Name);
            if (parameter != null)
            {
                return new InjectEntry { Provider = CreateParameterProvider(parameter), Setter = setter };
            }

            var propertyType = delegateFactory.GetExtendedPropertyType(pi);
            var constraint = ConstraintBuilder.Build(pi.GetCustomAttributes<ConstraintAttribute>());
            if (constraint != null)
            {
                return new InjectEntry { Provider = CreateConstraintProvider(propertyType, constraint), Setter = setter };
            }

            return new InjectEntry { Provider = CreateProvider(propertyType), Setter = setter };
        }

        private static Func<IResolver, object> CreateParameterProvider(IParameter parameter)
        {
            return parameter.Resolve;
        }

        private static Func<IResolver, object> CreateConstraintProvider(Type propertyType, IConstraint constraint)
        {
            return resolver => resolver.Get(propertyType, constraint);
        }

        private static Func<IResolver, object> CreateProvider(Type propertyType)
        {
            return resolver => resolver.Get(propertyType);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Performance")]
        private sealed class InjectEntry
        {
            public Func<IResolver, object> Provider;

            public Action<object, object> Setter;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Performance")]
        private sealed class Injector
        {
            public InjectEntry[] Entries;

            public void Inject(IResolver resolver, object instance)
            {
                for (var i = 0; i < Entries.Length; i++)
                {
                    var entry = Entries[i];
                    entry.Setter(instance, entry.Provider(resolver));
                }
            }
        }
    }
}
