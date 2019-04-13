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

        public Action<IKernel, object> CreateProcessor(Type type, IKernel kernel)
        {
            if (!BindableObjectType.IsAssignableFrom(type))
            {
                return null;
            }

            return (k, x) => (((BindableObject)x).BindingContext as IInitializable)?.Initialize();
        }
    }
}
