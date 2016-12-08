namespace Smart.Resolver
{
    using System;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Controllers;

    public class SmartResolverControllerActivator : IControllerActivator
    {
        private readonly IResolver resolver;

        public SmartResolverControllerActivator(IResolver resolver)
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
