namespace Smart.Resolver
{
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.AspNetCore.Mvc.ViewComponents;

    public static class ResolverConfigExtensions
    {
        public static ResolverConfig AddMvcActivatorSupport(this ResolverConfig config)
        {
            config.UseMissingHandler<ControllerMissingHandler>();
            config.UseMissingHandler<ViewComponentMissingHandler>();
            config.Bind<IControllerActivator>().To<SmartControllerActivator>().InSingletonScope();
            config.Bind<IViewComponentActivator>().To<SmartViewComponentActivator>().InSingletonScope();
            return config;
        }
    }
}
