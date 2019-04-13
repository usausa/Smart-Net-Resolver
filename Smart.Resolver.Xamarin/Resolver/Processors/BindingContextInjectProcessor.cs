namespace Smart.Resolver.Processors
{
    using System;

    using Xamarin.Forms;

    public sealed class BindingContextInjectProcessor : IProcessor
    {
        private static readonly Type BindableObjectType = typeof(BindableObject);

        public int Order { get; }

        public BindingContextInjectProcessor()
            : this(0)
        {
        }

        public BindingContextInjectProcessor(int order)
        {
            Order = order;
        }

        public Action<IKernel, object> CreateProcessor(Type type, IKernel kernel)
        {
            if (!BindableObjectType.IsAssignableFrom(type))
            {
                return null;
            }

            return (k, x) => kernel.Inject(((BindableObject)x).BindingContext);
        }
    }
}
