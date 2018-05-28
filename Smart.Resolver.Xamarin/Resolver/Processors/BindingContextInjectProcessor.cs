namespace Smart.Resolver.Processors
{
    using System;

    using Xamarin.Forms;

    public sealed class BindingContextInjectProcessor : IProcessor
    {
        private static readonly Type BindableObjectType = typeof(BindableObject);

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
