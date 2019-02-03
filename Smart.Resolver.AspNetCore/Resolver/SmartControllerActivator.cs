namespace Smart.Resolver
{
    using System;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Controllers;

    public sealed class SmartControllerActivator : IControllerActivator
    {
        private readonly SmartResolver resolver;

        public SmartControllerActivator(SmartResolver resolver)
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
