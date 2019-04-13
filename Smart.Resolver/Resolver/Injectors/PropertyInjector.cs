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

        public Action<IKernel, object> CreateInjector(Type type, IKernel kernel, IBinding binding)
        {
            var entries = type.GetRuntimeProperties()
                .Where(p => p.IsInjectDefined())
                .Select(x => CreateInjectEntry(x, binding))
                .ToArray();
            if (entries.Length == 0)
            {
                return null;
            }

            var injector = new Injector(entries);
            return injector.Inject;
        }

        private InjectEntry CreateInjectEntry(PropertyInfo pi, IBinding binding)
        {
            var setter = delegateFactory.CreateSetter(pi, true);

            var parameter = binding.PropertyValues.GetParameter(pi.Name);
            if (parameter != null)
            {
                return new InjectEntry(CreateParameterProvider(parameter), setter);
            }

            var propertyType = delegateFactory.GetExtendedPropertyType(pi);
            var constraint = ConstraintBuilder.Build(pi.GetCustomAttributes<ConstraintAttribute>());
            if (constraint != null)
            {
                return new InjectEntry(CreateConstraintProvider(propertyType, constraint), setter);
            }

            return new InjectEntry(CreateProvider(propertyType), setter);
        }

        private static Func<IKernel, object> CreateParameterProvider(IParameter parameter)
        {
            return parameter.Resolve;
        }

        private static Func<IKernel, object> CreateConstraintProvider(Type propertyType, IConstraint constraint)
        {
            return k => k.Get(propertyType, constraint);
        }

        private static Func<IKernel, object> CreateProvider(Type propertyType)
        {
            return k => k.Get(propertyType);
        }

        private sealed class InjectEntry
        {
            private readonly Func<IKernel, object> provider;

            private readonly Action<object, object> setter;

            public InjectEntry(Func<IKernel, object> provider, Action<object, object> setter)
            {
                this.provider = provider;
                this.setter = setter;
            }

            public void Inject(IKernel kernel, object instance)
            {
                setter(instance, provider(kernel));
            }
        }

        private sealed class Injector
        {
            private readonly InjectEntry[] entries;

            public Injector(InjectEntry[] entries)
            {
                this.entries = entries;
            }

            public void Inject(IKernel kernel, object instance)
            {
                for (var i = 0; i < entries.Length; i++)
                {
                    entries[i].Inject(kernel, instance);
                }
            }
        }
    }
}
