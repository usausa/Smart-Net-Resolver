namespace Smart.Resolver.Processors
{
    using System;

    using Xamarin.Forms;

    public sealed class BindingContextInitializeProcessor : IProcessor
    {
        private static readonly Type BindableObjectType = typeof(BindableObject);

        public int Order { get; }

        public BindingContextInitializeProcessor()
            : this(Int32.MinValue)
        {
        }

        public BindingContextInitializeProcessor(int order)
        {
            Order = order;
        }

        public Action<IResolver, object> CreateProcessor(Type type)
        {
            if (!BindableObjectType.IsAssignableFrom(type))
            {
                return null;
            }

            return (r, x) => (((BindableObject)x).BindingContext as IInitializable)?.Initialize();
        }
    }
}
