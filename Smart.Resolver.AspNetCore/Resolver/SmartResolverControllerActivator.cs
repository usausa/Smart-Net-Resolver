namespace Smart.Resolver
{
    using System;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Controllers;

    public sealed class SmartResolverControllerActivator : IControllerActivator
    {
        private readonly SmartResolver resolver;

        public SmartResolverControllerActivator(SmartResolver resolver)
        {
            this.resolver = resolver;
        }

        public object Create(ControllerContext context)
        {
            return resolver.Get(context.ActionDescriptor.ControllerTypeInfo.AsType());
        }

        public void Release(ControllerContext context, object controller)
        {
            (controller as IDisposable)?.Dispose();
        }
    }
}
