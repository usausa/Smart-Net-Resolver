namespace Smart.Resolver.CompatibilityTest;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Specification;

using Smart.Resolver;

public sealed class DependencyInjectionComplianceTest : DependencyInjectionSpecificationTests
{
    public override bool SupportsIServiceProviderIsService => true;

    protected override IServiceProvider CreateServiceProvider(IServiceCollection serviceCollection)
    {
        var factory = new SmartServiceProviderFactory();
        var config = factory.CreateBuilder(serviceCollection);
        return factory.CreateServiceProvider(config);
    }
}
