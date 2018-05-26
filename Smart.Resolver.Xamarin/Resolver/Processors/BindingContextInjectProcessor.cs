namespace Smart.Resolver.Processors
{
    using System;

    using Smart.Resolver.Injectors;

    using Xamarin.Forms;

    public sealed class BindingContextInjectProcessor : IProcessor
    {
        private static readonly Type BindableObjectType = typeof(BindableObject);

        private readonly IInjector[] injectors;

        public BindingContextInjectProcessor(IInjector[] injectors)
        {
            this.injectors = injectors;
        }

        public bool IsTarget(Type type)
        {
            return BindableObjectType.IsAssignableFrom(type);
        }

        public void Initialize(object instance)
        {
            var binding = new Smart.Resolver.Bindings.Binding(instance.GetType());
            for (var i = 0; i < injectors.Length; i++)
            {
                injectors[i].Initialize(binding, instance);
            }
        }
    }
}
