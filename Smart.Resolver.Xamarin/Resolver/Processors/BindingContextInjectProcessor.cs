namespace Smart.Resolver.Processors
{
    using System;

    using Xamarin.Forms;

    public sealed class BindingContextInjectProcessor : IProcessor
    {
        private static readonly Type BindableObjectType = typeof(BindableObject);

        public bool IsTarget(Type type)
        {
            return BindableObjectType.IsAssignableFrom(type);
        }

        public Action<object> CreateProcessor(IKernel kernel)
        {
            return x => kernel.Inject(((BindableObject)x).BindingContext);
        }
    }
}
