namespace Smart.Resolver.Processors
{
    using System;

    using Xamarin.Forms;

    public sealed class BindingContextProcessor : IProcessor
    {
        private static readonly Type BindableObjectType = typeof(BindableObject);

        public int Order { get; }

        public BindingContextProcessor()
            : this(0)
        {
        }

        public BindingContextProcessor(int order)
        {
            Order = order;
        }

        public Action<object> CreateProcessor(Type type, IKernel kernel)
        {
            if (!BindableObjectType.IsAssignableFrom(type))
            {
                return null;
            }

            return x => kernel.Inject(((BindableObject)x).BindingContext);
        }
    }
}
