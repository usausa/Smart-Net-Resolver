namespace Smart.Resolver
{
    using System;

    using Microsoft.AspNetCore.Mvc.ViewComponents;

    public sealed class SmartResolverViewComponentActivator : IViewComponentActivator
    {
        private readonly SmartResolver resolver;

        public SmartResolverViewComponentActivator(SmartResolver resolver)
        {
            this.resolver = resolver;
        }

        public object Create(ViewComponentContext context)
        {
            return resolver.Get(context.ViewComponentDescriptor.TypeInfo.AsType());
        }

        public void Release(ViewComponentContext context, object viewComponent)
        {
            (viewComponent as IDisposable)?.Dispose();
        }
    }
}
