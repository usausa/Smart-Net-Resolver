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

        public Action<IResolver, object> CreateProcessor(Type type)
        {
            if (!BindableObjectType.IsAssignableFrom(type))
            {
                return null;
            }

            return (r, x) => r.Inject(((BindableObject)x).BindingContext);
        }
    }
}
