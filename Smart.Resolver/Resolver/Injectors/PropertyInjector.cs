namespace Smart.Resolver.Injectors
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Smart.Reflection;
    using Smart.Resolver.Attributes;
    using Smart.Resolver.Bindings;
    using Smart.Resolver.Constraints;
    using Smart.Resolver.Helpers;
    using Smart.Resolver.Parameters;

    public sealed class PropertyInjector : IInjector
    {
        private static readonly Type InjectType = typeof(InjectAttribute);

        private readonly IDelegateFactory delegateFactory;

        public PropertyInjector(IDelegateFactory delegateFactory)
        {
            this.delegateFactory = delegateFactory;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public Action<object> CreateInjector(Type type, IKernel kernel, IBinding binding)
        {
            var entries = type.GetRuntimeProperties()
                .Where(p => p.IsDefined(InjectType))
                .Select(x => CreateInjectEntry(x, kernel, binding))
                .ToArray();
            if (entries.Length == 0)
            {
                return null;
            }

            var injector = new Injector(entries);
            return injector.Inject;
        }

        private InjectEntry CreateInjectEntry(PropertyInfo pi, IKernel kernel, IBinding binding)
        {
            var setter = delegateFactory.CreateSetter(pi, true);

            var parameter = binding.PropertyValues.GetParameter(pi.Name);
            if (parameter != null)
            {
                return new InjectEntry(CreateParameterProvider(kernel, parameter), setter);
            }

            var propertyType = delegateFactory.GetExtendedPropertyType(pi);
            var constraint = ConstraintHelper.CreateConstraint(pi.GetCustomAttributes<ConstraintAttribute>());
            if (constraint != null)
            {
                return new InjectEntry(CreateConstraintProvider(kernel, propertyType, constraint), setter);
            }

            return new InjectEntry(CreateProvider(kernel, propertyType), setter);
        }

        private static Func<object> CreateParameterProvider(IKernel kernel, IParameter parameter)
        {
            return () => parameter.Resolve(kernel);
        }

        private static Func<object> CreateConstraintProvider(IResolver resolver, Type propertyType, IConstraint constraint)
        {
            return () => resolver.Get(propertyType, constraint);
        }

        private static Func<object> CreateProvider(IResolver resolver, Type propertyType)
        {
            return () => resolver.Get(propertyType);
        }

        private sealed class InjectEntry
        {
            private readonly Func<object> provider;

            private readonly Action<object, object> setter;

            public InjectEntry(Func<object> provider, Action<object, object> setter)
            {
                this.provider = provider;
                this.setter = setter;
            }

            public void Inject(object instance)
            {
                setter(instance, provider());
            }
        }

        private sealed class Injector
        {
            private readonly InjectEntry[] entries;

            public Injector(InjectEntry[] entries)
            {
                this.entries = entries;
            }

            public void Inject(object instance)
            {
                for (var i = 0; i < entries.Length; i++)
                {
                    entries[i].Inject(instance);
                }
            }
        }
    }
}
